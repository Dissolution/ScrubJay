using System.Buffers;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using ScrubJay.Testing.Things;
#pragma warning disable IDE0004, IDE1006


namespace ScrubJay.Testing.Naughty;

public static class NaughtyObjects
{
    public static MemoryStream DisposedStream()
    {
        var stream = new MemoryStream([1, 4, 7]);
        stream.Dispose();
        return stream;
    }

    public static IEnumerable<object?> BoxedNullLikes()
    {
        // actual null
        yield return (object?)null;

        // a null Nullable<> boxes to actual null
        yield return (object?)(int?)null;

        // whereas a filled Nullable<> boxes to the underlying value
        yield return (object?)(int?)147;

        // DBNull, archaic
        yield return (object?)DBNull.Value;

        // a plain object that means nothing
        yield return new object();
    }

    public static IEnumerable<object> BoxedNumerics()
    {
        yield return (object)(byte)1;
        yield return (object)(sbyte)1;
        yield return (object)(short)1;
        yield return (object)(ushort)1;
        yield return (object)(int)1;
        yield return (object)(uint)1U;
        yield return (object)(long)1L;
        yield return (object)(ulong)1UL;
        yield return (object)(nint)1;
        yield return (object)(nuint)1;
        yield return (object)(Int128)1;
        yield return (object)(UInt128)1UL;
        yield return (object)(BigInteger)BigInteger.One;
        yield return (object)(Half)0.2;
        yield return (object)(float)1.4f;
        yield return (object)(double)2.6d;
        yield return (object)(decimal)3.8m;
        yield return (object)(bool)true;
        yield return (object)(char)'J';
    }

    public static IEnumerable<object> BoxedFloatingPoints()
    {
        yield return Half.NaN;
        yield return Half.Epsilon;
        yield return Half.NegativeInfinity;
        yield return Half.MinValue;
        yield return Half.NegativeOne;
        yield return Half.NegativeZero;
        yield return Half.Zero;
        yield return Half.One;
        yield return Half.MaxValue;
        yield return Half.PositiveInfinity;

        yield return float.NaN;
        yield return float.Epsilon;
        yield return float.NegativeInfinity;
        yield return float.MinValue;
        yield return -1.0f; //float.NegativeOne;
        yield return float.NegativeZero;
        yield return 0.0f; //float.Zero;
        yield return 1.0f; //float.One;
        yield return float.MaxValue;
        yield return float.PositiveInfinity;

        yield return double.NaN;
        yield return double.Epsilon;
        yield return double.NegativeInfinity;
        yield return double.MinValue;
        yield return -1.0d; //double.NegativeOne;
        yield return double.NegativeZero;
        yield return 0.0d; //double.Zero;
        yield return 1.0d; //double.One;
        yield return double.MaxValue;
        yield return double.PositiveInfinity;

        yield return 0.0000000000000000000000000001m; // smallest positive decimal
        yield return decimal.MinValue;
        yield return -1.0m; //decimal.NegativeOne;
        yield return -0.0m; //decimal.NegativeZero;
        yield return decimal.Zero;
        yield return decimal.One;
        yield return decimal.MaxValue;

        // not exactly equal to 0.3
        yield return (Half)0.1f + (Half)0.2f;
        yield return 0.1f + 0.2f;
        yield return 0.1d + 0.2d;
    }

    public static IEnumerable<object> BoxedIntegers()
    {
        yield return int.MinValue;
        yield return int.MaxValue;
        yield return uint.MaxValue;
        yield return long.MinValue;
        yield return 9_007_199_254_740_993L; // slightly too big for double
        yield return long.MaxValue;
        yield return ulong.MaxValue;
        yield return Int128.MinValue;
        yield return Int128.MaxValue;
        yield return UInt128.MaxValue;
        yield return BigInteger.Pow(10, 147);
    }

    public static IEnumerable<object> BoxedCharacters()
    {
        foreach (var ch in TestCharacters.Ascii.CONTROL_NON_WHITESPACE)
            yield return ch;

        yield return '\uD800'; // lone high surrogate
        yield return '\uDC00'; // lone low surrogate
        yield return char.MaxValue;
        yield return 'a'; // this is not an `a`
    }
    
    public static IEnumerable<object> BoxedStrings()
    {
        yield return "";
        yield return " ";
        yield return "_\0_";
        yield return "J";
        yield return new string('J', 1);
        yield return "TRJ";
        yield return new char[3] { 'T', 'R', 'J' };
        yield return (IEnumerable<char>)"TRJ".ToCharArray();
    }

//    private static IEnumerable<NaughtyObject> Enums()
//    {
//        const string c = "Enums";
//        yield return new(c, "(DayOfWeek)42", "Undefined value; Enum.IsDefined is false and ToString returns \"42\".", (DayOfWeek)42);
//        yield return new(c, "DayOfWeek.Sunday", "Value 0; indistinguishable from default.", DayOfWeek.Sunday);
//        yield return new(c, "DayOfWeek.Monday", "Unboxes as int even though it is not an int.", DayOfWeek.Monday);
//        yield return new(c, "Flags combination", "Not a single defined name; ToString returns \"Read, Write\".", NaughtyEnums.NaughtyPermissions.Read | NaughtyEnums.NaughtyPermissions.Write);
//        yield return new(c, "Flags undefined bit", "A bit with no name; ToString returns a number.", (NaughtyEnums.NaughtyPermissions)64);
//        yield return new(c, "duplicate names", "Two names share one value; which name ToString picks is unspecified.", NaughtyDuplicateNames.Primary);
//        yield return new(c, "no zero member", "default is 0, which has no name.", default(NaughtyNoZero));
//        yield return new(c, "long-backed enum", "Value does not fit in int; unboxing as int throws.", NaughtyLongEnum.Huge);
//        yield return new(c, "byte-backed enum", "Unboxes only as byte, not int.", NaughtyByteEnum.Max);
//    }

    private static IEnumerable<NaughtyObject> DatesAndTimes()
    {
        const string c = "Dates and times";
        yield return new(c, "DateTime.MinValue", "Converting to DateTimeOffset throws in UTC+ time zones.", DateTime.MinValue);
        yield return new(c, "DateTime.MaxValue", "Converting to DateTimeOffset throws in UTC- time zones.", DateTime.MaxValue);
        yield return new(c, "2026-01-01 Utc", "Equal to the Local and Unspecified values with the same ticks, despite meaning a different instant.", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        yield return new(c, "2026-01-01 Local", "Same ticks as the Utc value; compares equal.", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local));
        yield return new(c, "2026-01-01 Unspecified", "Same ticks as the Utc value; treated as local by most conversions.", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
        yield return new(c, "2026-11-01 01:30 Unspecified", "Ambiguous local time in US time zones (DST ends).", new DateTime(2026, 11, 1, 1, 30, 0, DateTimeKind.Unspecified));
        yield return new(c, "2026-03-08 02:30 Unspecified", "Nonexistent local time in US time zones (DST starts).", new DateTime(2026, 3, 8, 2, 30, 0, DateTimeKind.Unspecified));
        yield return new(c, "DateTimeOffset.MinValue", "Converting to local time can underflow.", DateTimeOffset.MinValue);
        yield return new(c, "TimeSpan.MinValue", "Duration() and Negate() throw OverflowException.", TimeSpan.MinValue);
        yield return new(c, "TimeSpan.MaxValue", "Adding it to any DateTime throws.", TimeSpan.MaxValue);
        yield return new(c, "DateOnly.MinValue", "Not IConvertible; older serializers do not know the type.", DateOnly.MinValue);
        yield return new(c, "TimeOnly.MaxValue", "Not IConvertible; 23:59:59.9999999 often truncates to the wrong second.", TimeOnly.MaxValue);
    }

    private static IEnumerable<NaughtyObject> DefaultStructs()
    {
        const string c = "Default structs";
        yield return new(c, "default(ImmutableArray<int>)", "Not empty but uninitialized; almost every member throws, and only IsDefault tells you.", default(ImmutableArray<int>));
        yield return new(c, "ImmutableArray<int>.Empty", "The valid empty value, for contrast with default.", ImmutableArray<int>.Empty);
        yield return new(c, "default(ArraySegment<int>)", "Its Array property is null.", default(ArraySegment<int>));
        yield return new(c, "default(KeyValuePair<string, int>)", "Key is null in a type whose keys are usually non-null.", default(KeyValuePair<string, int>));
        yield return new(c, "Guid.Empty", "All zeros; usually means \"missing\" but passes null checks.", Guid.Empty);
        yield return new(c, "default(CancellationToken)", "CanBeCanceled is false; registering callbacks does nothing.", default(CancellationToken));
    }

    private static IEnumerable<NaughtyObject> Arrays()
    {
        const string c = "Arrays";
        yield return new(c, "Array.Empty<int>()", "Empty; First(), Max(), and indexing [0] throw.", Array.Empty<int>());
        yield return new(c, "int[,]", "Multidimensional; implements IEnumerable but not IEnumerable<int>.", new[,] { { 1, 2 }, { 3, 4 } });
        yield return new(c, "int[][] with a null row", "Jagged, with a null inner array.", new[] { new[] { 1 }, null });
        yield return new(c, "non-zero lower bound", "Type is Int32[*], not Int32[]; it is not an IList<int>.", Array.CreateInstance(typeof(int), new[] { 1 }, new[] { 1 }));
        yield return new(c, "int[] { -1 }", "The runtime says this is also a uint[], and casting it to one succeeds.", new[] { -1 });
        yield return new(c, "string[] as object[]", "Array covariance: writing a non-string into it throws ArrayTypeMismatchException.", new[] { "a" });
        yield return new(c, "byte[100_000]", "Lands on the large object heap; slow to copy, log, or serialize.", new byte[100_000]);
    }

    private static IEnumerable<NaughtyObject> Collections()
    {
        const string c = "Collections";
        yield return new(c, "ReadOnlyCollection<int>", "Implements IList<int>, but Add throws NotSupportedException.", new ReadOnlyCollection<int>(new[] { 1 }));
        yield return new(c, "List<string>", "Is an IEnumerable<object> through variance.", new List<string> { "a" });
        yield return new(c, "List<int>", "Is not an IEnumerable<object>; variance does not apply to value types.", new List<int> { 1 });
        yield return new(c, "List<object?> of mixed nulls", "Elements of different types plus nulls.", new List<object?> { 1, "1", null, 1L });
        yield return new(c, "Dictionary, case-insensitive", "Same keys as a default-comparer dictionary, different lookup behavior.", new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["A"] = 1 });
        yield return new(c, "Dictionary, default comparer", "Is an IEnumerable<KeyValuePair<string, int>>, which `is IEnumerable` checks catch.", new Dictionary<string, int> { ["A"] = 1 });
        yield return new(c, "Hashtable { 1, 1L }", "Two separate keys that both look like 1.", new Hashtable { [1] = "int", [1L] = "long" });
        yield return new(c, "ArrayList", "Non-generic; elements are object.", new ArrayList { 1, "1", null });
        yield return new(c, "BitArray", "Non-generic IEnumerable of bool.", new BitArray(3));
        yield return new(c, "HashSet with mutated key", "A key mutated after insertion; Contains returns false for an element it holds.", MutatedKeySet());
    }

    private static IEnumerable<NaughtyObject> Enumerables()
    {
        const string c = "Enumerables";
        yield return new(c, "infinite", "ToList, Count, and serialization never return.", Infinite());
        yield return new(c, "single pass", "The second enumeration throws InvalidOperationException.", new SinglePassEnumerable());
        yield return new(c, "throws after two items", "Fails partway through, after side effects on the first items.", ThrowsAfterTwo());
        yield return new(c, "different every enumeration", "A lazy LINQ query; enumerating twice yields different values.", Enumerable.Range(0, 3).Select(_ => Guid.NewGuid()));
    }

    private static IEnumerable<NaughtyObject> Graphs()
    {
        const string c = "Graphs";
        var selfList = new List<object?>();
        selfList.Add(selfList);
        yield return new(c, "list containing itself", "Breaks serializers, deep equality, and recursive ToString.", selfList);

        var node = new CyclicNode();
        node.Next = node;
        yield return new(c, "cyclic object", "An object whose property points back to itself.", node);

        yield return new(c, "infinitely deep property", "Each read returns a new child, so the graph has no cycle and no bottom.", new InfinitelyDeep());
        yield return new(c, "nested 1,000 deep", "Exceeds most serializer depth limits (System.Text.Json defaults to 64) without overflowing the stack.", Nested(1_000));
    }

    private static IEnumerable<NaughtyObject> HostileObjects()
    {
        const string c = "Hostile objects";
        yield return new(c, "ToString returns null", "Legal; breaks code that calls .Length on the result.", new NullToString());
        yield return new(c, "ToString throws", "Breaks logging, debugger displays, and string interpolation.", new ThrowingToString());
        yield return new(c, "never equal to itself", "Equals returns false even for the same reference.", new NeverEqualToSelf());
        yield return new(c, "Equals throws", "Breaks Contains, IndexOf, Distinct, and dictionary lookups.", new ThrowingEquals());
        yield return new(c, "GetHashCode throws", "Breaks any hashed collection.", new ThrowingGetHashCode());
        yield return new(c, "GetHashCode changes", "Returns a new value on every call; hashed collections lose it.", new ChangingHashCode());
        yield return new(c, "== disagrees with Equals", "== always returns true; Equals is reference equality.", new OperatorDisagreesWithEquals());
        yield return new(c, "property getter throws", "Breaks reflection-based serializers and debuggers.", new ThrowingProperty());
        yield return new(c, "Dispose throws", "Breaks using blocks and cleanup loops.", new ThrowingDispose());
        yield return new(c, "already disposed stream", "Every member throws ObjectDisposedException.", DisposedStream());
        yield return new(c, "indexer", "Reflection sees a property named Item with parameters; GetValue without an index throws.", new HasIndexer());
        yield return new(c, "property hidden with new", "GetProperty(\"Value\") throws AmbiguousMatchException.", new HidingDerived());
        yield return new(c, "explicit interface implementation", "CompareTo is invisible through the class type.", new ExplicitComparable());
    }

    private static IEnumerable<NaughtyObject> Conversions()
    {
        const string c = "Conversions";
        yield return new(c, "implicit conversion operators", "Converts to double on a typed variable, but casting from object throws InvalidCastException.", new Meters(5));
        yield return new(c, "DynamicObject that lies", "Claims every member exists and every conversion succeeds, but only through dynamic.", new LyingDynamicObject());
        yield return new(c, "ExpandoObject", "Is also an IDictionary<string, object?>.", Expando());
        yield return new(c, "Version 1.0", "Not equal to Version 1.0.0.", new Version(1, 0));
        yield return new(c, "relative Uri", "Host and most other properties throw InvalidOperationException.", new Uri("/relative/path", UriKind.Relative));
        yield return new(c, "CultureInfo tr-TR", "Turkish dotless i; case-insensitive comparisons of \"i\" and \"I\" fail.", CultureInfo.GetCultureInfo("tr-TR"));
    }

    private static IEnumerable<NaughtyObject> RuntimeTypes()
    {
        const string c = "Runtime types";
        yield return new(c, "typeof(int)", "A Type is an object too, of runtime type RuntimeType.", typeof(int));
        yield return new(c, "typeof(List<>)", "Open generic; Activator.CreateInstance throws.", typeof(List<>));
        yield return new(c, "typeof(int).MakeByRefType()", "By-ref type; cannot be instantiated or used as a generic argument.", typeof(int).MakeByRefType());
        yield return new(c, "typeof(int).MakePointerType()", "Pointer type; cannot be boxed or used as a generic argument.", typeof(int).MakePointerType());
        yield return new(c, "typeof(void)", "Not a type any value can have.", typeof(void));
        yield return new(c, "anonymous type", "Internal; dynamic access from another assembly throws RuntimeBinderException.", new { Id = 1, Name = (string?)null });
        yield return new(c, "ValueTuple of 9", "Elements past the seventh nest inside Rest.", (1, 2, 3, 4, 5, 6, 7, 8, 9));
        yield return new(c, "Tuple with null", "Reference tuple holding a null.", Tuple.Create<int, string?>(1, null));
    }

    private static IEnumerable<NaughtyObject> Delegates()
    {
        const string c = "Delegates";
        Action multicast = () => { };
        multicast += () => { };
        yield return new(c, "multicast Action", "Two invocation targets; Invoke returns only the last result for non-void delegates.", multicast);

        var captured = 42;
        Func<int> closure = () => captured;
        yield return new(c, "closure", "Target is a compiler-generated class.", closure);

        Func<int> throwing = () => throw new InvalidOperationException("Naughty delegate.");
        yield return new(c, "throwing Func<int>", "Invoking it throws.", throwing);
    }

    private static IEnumerable<NaughtyObject> Tasks()
    {
        const string c = "Tasks";
        yield return new(c, "Task.CompletedTask", "Not a Task<T>; has no Result.", Task.CompletedTask);
        yield return new(c, "Task.FromResult(1)", "Completed Task<int>.", Task.FromResult(1));
        yield return new(c, "faulted Task", "Awaiting or reading Result throws; if never observed, raises TaskScheduler.UnobservedTaskException on finalization.", Task.FromException(new InvalidOperationException("Naughty task.")));
        yield return new(c, "canceled Task", "Awaiting throws TaskCanceledException.", Task.FromCanceled(new CancellationToken(canceled: true)));
        yield return new(c, "never-completing Task", "Awaiting hangs; reading Result blocks forever.", new TaskCompletionSource().Task);
        yield return new(c, "boxed ValueTask<int>", "A struct pretending to be a task; awaiting one backed by IValueTaskSource twice is undefined behavior.", new ValueTask<int>(1));
        yield return new(c, "Lazy<int> whose factory throws", "Reading Value throws, and keeps throwing (the exception is cached).", new Lazy<int>(() => throw new InvalidOperationException("Naughty lazy.")));
    }

    private static IEnumerable<NaughtyObject> Exceptions()
    {
        const string c = "Exceptions";
        yield return new(c, "never-thrown exception", "StackTrace is null and TargetSite is null.", new InvalidOperationException("Never thrown."));
        yield return new(c, "nested AggregateException", "An AggregateException inside another; Flatten is needed to see the leaves.", new AggregateException(new AggregateException(new InvalidOperationException("Inner.")), new ArgumentException("Sibling.")));
        yield return new(c, "inner-exception chain of 100", "Long chains break loggers that walk InnerException recursively.", ChainedException(100));
    }

    private static IEnumerable<NaughtyObject> Miscellaneous()
    {
        const string c = "Miscellaneous";
        yield return new(c, "WeakReference with no target", "IsAlive is false and Target is null.", new WeakReference(null));
        yield return new(c, "StrongBox<object?> holding itself", "A one-field cycle through a public field, which some serializers include and others skip.", SelfBox());
    }

    private static IEnumerable<NaughtyObject> SpanRelatives()
    {
        const string c = "Span relatives";
        yield return new(c, "default(Memory<int>)", "Empty, and its Span is a default span.", default(Memory<int>));
        yield return new(c, "ReadOnlyMemory<char> slice of a string", "MemoryMarshal.TryGetString returns the whole string plus a start of 6; code that ignores the start reads the wrong text.", "hello world".AsMemory(6));
        yield return new(c, "ReadOnlySequence<byte>.Empty", "Zero length; First and FirstSpan are empty.", ReadOnlySequence<byte>.Empty);
        yield return new(c, "multi-segment ReadOnlySequence<byte>", "IsSingleSegment is false, and FirstSpan holds only part of the data.", NaughtySegment<byte>.Sequence(new byte[] { 1, 2 }, new byte[] { 3, 4, 5 }));
        yield return new(c, "ReadOnlySequence<byte> with an empty middle segment", "Valid, but loops that stop at the first empty span end early.", NaughtySegment<byte>.Sequence(new byte[] { 1, 2 }, Array.Empty<byte>(), new byte[] { 3, 4 }));
        yield return new(c, "boxed null int*", "A pointer boxed as System.Reflection.Pointer; GetType() is Pointer, not int*.", BoxedNullPointer());
    }

    private static HashSet<MutableKey> MutatedKeySet()
    {
        var key = new MutableKey { Id = 1 };
        var set = new HashSet<MutableKey> { key };
        key.Id = 2;
        return set;
    }

    private static IEnumerable<int> Infinite()
    {
        var i = 0;
        while (true)
        {
            yield return i++;
        }
    }

    private static IEnumerable<int> ThrowsAfterTwo()
    {
        yield return 1;
        yield return 2;
        throw new InvalidOperationException("Naughty enumerable.");
    }

    private static List<object?> Nested(int depth)
    {
        var root = new List<object?>();
        var current = root;
        for (var i = 1; i < depth; i++)
        {
            var child = new List<object?>();
            current.Add(child);
            current = child;
        }

        return root;
    }


    private static ExpandoObject Expando()
    {
        var expando = new ExpandoObject();
        ((IDictionary<string, object?>)expando)["Name"] = "expando";
        return expando;
    }

    private static Exception ChainedException(int length)
    {
        Exception current = new InvalidOperationException("Innermost.");
        for (var i = 1; i < length; i++)
        {
            current = new InvalidOperationException($"Level {i}.", current);
        }

        return current;
    }

    private static StrongBox<object?> SelfBox()
    {
        var box = new StrongBox<object?>();
        box.Value = box;
        return box;
    }

    private static unsafe object BoxedNullPointer() => Pointer.Box(null, typeof(int*));
}