using System.Buffers;
using System.Text.Json;

namespace ScrubJay.Testing.Naughty;

/// <summary>
/// Problem values that cannot be boxed: spans, other ref structs, and <see cref="TypedReference"/>.
/// </summary>
/// <remarks>
/// Each case is a boxable recipe, so the lists work as xUnit <c>MemberData</c>. <c>Run</c> builds the value
/// inside its own stack frame and passes it to the consumer, so stack-allocated and native-memory cases stay
/// valid for exactly the duration of the call. Write one parameterized test per method under test. A method
/// that takes <c>ReadOnlySpan&lt;T&gt;</c> can also use <see cref="Spans{T}"/>, because a span converts
/// implicitly. Not covered: <see cref="ArgIterator"/> and <see cref="RuntimeArgumentHandle"/> values, which
/// exist only inside <c>__arglist</c> varargs methods; add cases only if you own such a method.
/// </remarks>
public static class NaughtySpanList
{
    /// <summary>
    /// Span shapes that are independent of element values. Elements are filled with a nonzero byte pattern,
    /// which for <see cref="bool"/> produces non-canonical true values (bytes other than 1).
    /// </summary>
    public static IReadOnlyList<NaughtyCase<NaughtySpanConsumer<T>>> Spans<T>()
        where T : unmanaged
    {
        const string c = "Spans";
        var list = new List<NaughtyCase<NaughtySpanConsumer<T>>>
        {
            new(c, "default", "Same as Span<T>.Empty; fixed gives a null pointer and MemoryMarshal.GetReference returns a null reference.", consumer => consumer(default)),
#pragma warning disable IDE0301
            new(c, "empty over an array", "Also empty, but == against default is false, because == compares the underlying reference as well as the length. Use IsEmpty.", consumer => consumer(Array.Empty<T>())),
#pragma warning restore IDE0301
            new(c, "one element over a local", "Backed by a single stack variable rather than an array, so it cannot become an array or Memory<T>.", consumer =>
            {
                var local = default(T);
                consumer(new Span<T>(ref local));
            }),
            new(c, "middle of a larger array", "A view into a shared array; writes land in memory the caller may still be using.", consumer => consumer(Patterned<T>(16).AsSpan(3, 5))),
            new(c, "ends at the array's end", "Any read past Length leaves the array.", consumer => consumer(Patterned<T>(16).AsSpan(8))),
            new(c, "stackalloc", "Stack memory; it cannot be stored or captured, and is garbage once the frame returns.", consumer =>
            {
                Span<T> span = stackalloc T[8];
                consumer(span);
            }),
            new(c, "native memory", "Unmanaged memory, freed right after the call; anything that keeps a pointer to it reads freed memory.", consumer => WithNative(8, consumer)),
            new(c, "1,048,576 elements", "Far bigger than a thread's stack; code that does stackalloc T[span.Length] crashes the process.", consumer => consumer(new T[1 << 20])),
        };

        foreach (var length in new[] { 1, 3, 7, 15, 17, 31, 33, 63, 65 })
        {
            list.Add(new(c, $"length {length}", "Not a whole number of vectors, so vectorized code must handle a scalar tail.", consumer => consumer(Patterned<T>(length))));
        }

        return list;
    }

    public static IReadOnlyList<NaughtyCase<NaughtyReadOnlySpanConsumer<char>>> CharSpans()
    {
        const string c = "Char spans";
        return new List<NaughtyCase<NaughtyReadOnlySpanConsumer<char>>>
        {
            new(c, "default", "Empty, with a null reference underneath.", consumer => consumer(default)),
            new(c, "slice of a string", "Not null-terminated, unlike a whole string; passing it to native code through fixed reads past the end.", consumer => consumer("prefix:value:suffix".AsSpan(7, 5))),
            new(c, "ends in a lone high surrogate", "Slicing split a surrogate pair.", consumer => consumer("a\uD83D\uDE00b".AsSpan(0, 2))),
            new(c, "starts with a lone low surrogate", "Slicing split a surrogate pair at the other end.", consumer => consumer("a\uD83D\uDE00b".AsSpan(2))),
            new(c, "embedded null on the stack", "Native code and some loggers stop at the null character.", consumer =>
            {
                ReadOnlySpan<char> span = stackalloc char[] { 'a', '\0', 'b' };
                consumer(span);
            }),
            new(c, "native memory of nulls", "Eight null characters in unmanaged memory, freed right after the call.", consumer => WithNative<char>(8, span => consumer(span))),
            new(c, "1,048,576 chars", "Too large to stackalloc a matching buffer.", consumer => consumer(new string('a', 1 << 20))),
        };
    }

    public static IReadOnlyList<NaughtyCase<NaughtyReadOnlySpanConsumer<byte>>> ByteSpans()
    {
        const string c = "Byte spans";
        return new List<NaughtyCase<NaughtyReadOnlySpanConsumer<byte>>>
        {
            new(c, "default", "Empty, with a null reference underneath.", consumer => consumer(default)),
            new(c, "lone UTF-8 lead byte", "0xC3 with no continuation byte; invalid UTF-8.", consumer => consumer(new byte[] { 0xC3 })),
            new(c, "overlong UTF-8", "0xC0 0xAF is an overlong '/'; decoders that accept it allow path-traversal bypasses.", consumer => consumer(new byte[] { 0xC0, 0xAF })),
            new(c, "UTF-8-encoded surrogate", "0xED 0xA0 0x80 encodes U+D800, which UTF-8 forbids (CESU-8 style).", consumer => consumer(new byte[] { 0xED, 0xA0, 0x80 })),
            new(c, "0xFF", "Never valid anywhere in UTF-8.", consumer => consumer(new byte[] { 0xFF })),
            new(c, "truncated 4-byte sequence", "The first three bytes of an emoji; streaming decoders must wait for more input.", consumer => consumer(new byte[] { 0xF0, 0x9F, 0x98 })),
            new(c, "UTF-8 BOM", "Decoders that do not strip it yield U+FEFF before the text.", consumer => consumer(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 })),
            new(c, "UTF-16LE BOM", "UTF-16 text where UTF-8 is expected.", consumer => consumer(new byte[] { 0xFF, 0xFE, 0x61, 0x00 })),
            new(c, "unaligned", "Starts one byte into its array, so reads of wider types are unaligned; Unsafe.Read<T> requires alignment and Unsafe.ReadUnaligned does not.", consumer => consumer(Patterned<byte>(9).AsSpan(1))),
            new(c, "length 7", "MemoryMarshal.Cast<byte, int> silently drops the last 3 bytes.", consumer => consumer(Patterned<byte>(7))),
        };
    }

    public static IReadOnlyList<NaughtyCase<NaughtyReadOnlySpanConsumer<object?>>> ObjectSpans()
    {
        const string c = "Object spans";
        return new List<NaughtyCase<NaughtyReadOnlySpanConsumer<object?>>>
        {
            new(c, "default", "Empty, with a null reference underneath.", consumer => consumer(default)),
            new(c, "all nulls", "Three null elements.", consumer => consumer(new object?[3])),
            new(c, "backed by a string[]", "Fine as ReadOnlySpan<object?>, but creating a Span<object?> over the same array throws ArrayTypeMismatchException.", consumer => consumer((object?[])new[] { "a", "b" })),
            //new(c, "every boxable naughty object", "The whole NaughtyObjectList, as one span.", consumer => consumer(NaughtyObjects.CreateValues().ToArray())),
            new(c, "array containing itself", "A one-element cycle.", consumer =>
            {
                var array = new object?[1];
                array[0] = array;
                consumer(array);
            }),
        };
    }

    /// <summary>
    /// Source and destination pairs for copy, move, and transform routines.
    /// </summary>
    public static IReadOnlyList<NaughtyCase<NaughtyOverlapConsumer<T>>> Overlaps<T>()
        where T : unmanaged
    {
        const string c = "Overlaps";
        return new List<NaughtyCase<NaughtyOverlapConsumer<T>>>
        {
            new(c, "identical", "Source and destination are the same memory.", consumer =>
            {
                var array = Patterned<T>(8);
                consumer(array, array);
            }),
            new(c, "destination one ahead", "A naive forward copy smears the first element across the destination.", consumer =>
            {
                var array = Patterned<T>(16);
                consumer(array.AsSpan(0, 8), array.AsSpan(1, 8));
            }),
            new(c, "destination one behind", "A naive backward copy corrupts the result.", consumer =>
            {
                var array = Patterned<T>(16);
                consumer(array.AsSpan(1, 8), array.AsSpan(0, 8));
            }),
            new(c, "adjacent", "Touching but not overlapping; overlap checks that use <= instead of < misfire.", consumer =>
            {
                var array = Patterned<T>(16);
                consumer(array.AsSpan(0, 8), array.AsSpan(8, 8));
            }),
            new(c, "destination shorter", "CopyTo throws ArgumentException; TryCopyTo returns false.", consumer => consumer(Patterned<T>(8), new T[4])),
            new(c, "destination longer", "Elements past the source length keep stale values.", consumer => consumer(Patterned<T>(8), new T[16])),
        };
    }

    public static IReadOnlyList<NaughtyCase<NaughtyJsonReaderConsumer>> JsonReaders()
    {
        const string c = "Utf8JsonReader";
        return new List<NaughtyCase<NaughtyJsonReaderConsumer>>
        {
            new(c, "before the first Read", "TokenType is None until Read is called.", consumer =>
            {
                var reader = new Utf8JsonReader("{}"u8);
                consumer(ref reader);
            }),
            new(c, "empty final input", "Read throws JsonException.", consumer =>
            {
#pragma warning disable IDE0301
                var reader = new Utf8JsonReader(ReadOnlySpan<byte>.Empty);
#pragma warning restore IDE0301
                consumer(ref reader);
            }),
            new(c, "partial, not final", "Read returns false mid-document, which is not the end of the document; BytesConsumed says where to resume.", consumer =>
            {
                var reader = new Utf8JsonReader("{\"a\":"u8, isFinalBlock: false, state: default);
                consumer(ref reader);
            }),
            new(c, "tokens split across segments", "For split tokens, HasValueSequence is true and ValueSpan is empty.", consumer =>
            {
                var reader = new Utf8JsonReader(NaughtySegment<byte>.Sequence("{\"na"u8.ToArray(), "me\":\"va"u8.ToArray(), "lue\"}"u8.ToArray()));
                consumer(ref reader);
            }),
            new(c, "escaped property name", "ValueSpan holds the escaped bytes; compare with ValueTextEquals or GetString, not ValueSpan.", consumer =>
            {
                var reader = new Utf8JsonReader("{\"\\u0041\":1}"u8);
                consumer(ref reader);
            }),
            new(c, "lone surrogate escape", "Valid JSON, but GetString throws InvalidOperationException.", consumer =>
            {
                var reader = new Utf8JsonReader("\"\\uD800\""u8);
                consumer(ref reader);
            }),
            new(c, "number beyond ulong", "TryGetInt64 and TryGetUInt64 both return false.", consumer =>
            {
                var reader = new Utf8JsonReader("18446744073709551616"u8);
                consumer(ref reader);
            }),
            new(c, "comments allowed", "Produces Comment tokens that consumers must skip.", consumer =>
            {
                var reader = new Utf8JsonReader("/* note */ 1"u8, new JsonReaderOptions { CommentHandling = JsonCommentHandling.Allow });
                consumer(ref reader);
            }),
            new(c, "trailing comma allowed", "An array whose last element is followed by a comma.", consumer =>
            {
                var reader = new Utf8JsonReader("[1,]"u8, new JsonReaderOptions { AllowTrailingCommas = true });
                consumer(ref reader);
            }),
            new(c, "duplicate property names", "The reader accepts them; whether the first or last wins is up to the consumer.", consumer =>
            {
                var reader = new Utf8JsonReader("{\"a\":1,\"a\":2}"u8);
                consumer(ref reader);
            }),
            new(c, "65 levels deep", "Exceeds the default MaxDepth of 64, so Read throws partway through.", consumer =>
            {
                var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(new string('[', 65) + new string(']', 65)));
                consumer(ref reader);
            }),
        };
    }

    public static IReadOnlyList<NaughtyCase<NaughtySequenceReaderConsumer>> SequenceReaders()
    {
        const string c = "SequenceReader";
        return new List<NaughtyCase<NaughtySequenceReaderConsumer>>
        {
            new(c, "empty", "End is true immediately.", consumer =>
            {
                var reader = new SequenceReader<byte>(ReadOnlySequence<byte>.Empty);
                consumer(ref reader);
            }),
            new(c, "value split across segments", "A 4-byte value spans two segments, so code that reads only CurrentSpan fails; TryReadLittleEndian handles it.", consumer =>
            {
                var reader = new SequenceReader<byte>(NaughtySegment<byte>.Sequence(new byte[] { 1, 2 }, new byte[] { 3, 4, 5 }));
                consumer(ref reader);
            }),
            new(c, "empty middle segment", "Valid, but loops that stop at the first empty span end early.", consumer =>
            {
                var reader = new SequenceReader<byte>(NaughtySegment<byte>.Sequence(new byte[] { 1, 2 }, Array.Empty<byte>(), new byte[] { 3, 4 }));
                consumer(ref reader);
            }),
            new(c, "only empty segments", "Two segments and zero bytes.", consumer =>
            {
                var reader = new SequenceReader<byte>(NaughtySegment<byte>.Sequence(Array.Empty<byte>(), Array.Empty<byte>()));
                consumer(ref reader);
            }),
        };
    }

    public static IReadOnlyList<NaughtyCase<NaughtyTypedReferenceConsumer>> TypedReferences()
    {
        const string c = "TypedReference";
        return new List<NaughtyCase<NaughtyTypedReferenceConsumer>>
        {
            new(c, "int local", "Refers to a stack variable; TypedReference.ToObject boxes a copy.", consumer =>
            {
                var value = 42;
                consumer(__makeref(value));
            }),
            new(c, "null string local", "ToObject returns null, but GetTargetType still reports String.", consumer =>
            {
                string? value = null;
                consumer(__makeref(value));
            }),
            new(c, "struct local", "Refers to a multi-field struct on the stack.", consumer =>
            {
                var value = DateTime.MaxValue;
                consumer(__makeref(value));
            }),
            new(c, "array element", "Refers to an element inside a heap array.", consumer =>
            {
                var array = new[] { 1, 2, 3 };
                consumer(__makeref(array[1]));
            }),
            new(c, "field of a heap object", "Refers to a field inside a class instance.", consumer =>
            {
                var box = new StrongBox<int>(7);
                consumer(__makeref(box.Value));
            }),
        };
    }

    /// <summary>
    /// Ref struct types, for reflection-level checks such as <see cref="Type.IsByRefLike"/>. A generic method
    /// marked <c>allows ref struct</c> still needs one closed instantiation per type written in code.
    /// </summary>
    public static IReadOnlyList<Type> RefStructTypes() => new[]
    {
        typeof(Span<int>),
        typeof(ReadOnlySpan<char>),
        typeof(Utf8JsonReader),
        typeof(SequenceReader<byte>),
        typeof(TypedReference),
        typeof(ArgIterator),
        typeof(RuntimeArgumentHandle),
        typeof(NaughtyRefStruct),
    };

    private static T[] Patterned<T>(int length)
        where T : unmanaged
    {
        var array = new T[length];
        var bytes = MemoryMarshal.AsBytes(array.AsSpan());
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(i % 251 + 1);
        }

        return array;
    }

    private static unsafe void WithNative<T>(int length, NaughtySpanConsumer<T> consumer)
        where T : unmanaged
    {
        var pointer = NativeMemory.AllocZeroed((nuint)length, (nuint)sizeof(T));
        try
        {
            consumer(new Span<T>(pointer, length));
        }
        finally
        {
            NativeMemory.Free(pointer);
        }
    }
}