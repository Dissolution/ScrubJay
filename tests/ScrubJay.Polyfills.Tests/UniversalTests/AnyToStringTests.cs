//using ScrubJay.Polyfills.Tests.Things;
//using ScrubJay.Polyfills.Universal;
//using ScrubJay.Testing.Things;
//// ReSharper disable UseSymbolAlias
//
//namespace ScrubJay.Polyfills.Tests.UniversalTests;
//
//public class AnyToStringTests
//{
//    public static TheoryData<object?> TestObjects { get; } =
//    [
//        (object?)null,
//        (object?)new object(),
//        (object?)(Type)typeof(object),
//        (object?)(byte)13,
//        (object?)(long)147,
//        (object?)(char)'\0',
//        (object?)(string)"TRJ",
//        (object?)(Guid)Guid.Empty,
//        (object?)(DateTime)DateTime.Now,
//        (object?)(Tuple<Guid, string?>)new Tuple<Guid, string>(Guid.NewGuid(), "TRJ")!,
//        (object?)(ValueTuple<Guid, string?>)(Guid.NewGuid(), "TRJ")!,
//        (object?)(Array)(new char[] { 'T', 'J' }),
//        (object?)(int[])([1, 4, 7]),
//        (object?)(ClassThing)new ClassThing(),
//        (object?)(SealedClassThing)new SealedClassThing(),
//        (object?)(StructThing)new StructThing(),
//        (object?)(ReadonlyStructThing)new ReadonlyStructThing(),
//        (object?)(List<byte>)new List<byte>() { 1, 4, 7 },
//        (object?)(IDictionary<string, int>)new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase),
//    ];
//
//    public static TheoryData<string?> TestStrings { get; } =
//    [
//        (string?)null,
//        "",
//        "\0",
//        "\0\0\0",
//        "\n",
//        "\r\n",
//        "TRJ",
//        "The quick brown fox jumped over the lazy dogs.",
//    ];
//
//
//    [Theory]
//    [MemberData(nameof(TestObjects))]
//    public void ObjectWorks(object? obj)
//    {
//        string? anyToString = Any.ToString(obj);
//        if (obj is null)
//        {
//            Assert.Null(anyToString);
//            return;
//        }
//
//        Assert.NotNull(anyToString);
//        
//        string? toString = obj.ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//
//#if NET9_0_OR_GREATER
//    [Theory]
//    [MemberData(nameof(TestStrings))]
//    public void ReadOnlySpanCharWorks(string? testString)
//    {
//        text text = testString;
//
//        string? anyToString = Any.ToString<ReadOnlySpan<char>>(text);
//        Assert.NotNull(anyToString);
//
//        string? toString = text.ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//
//    [Theory]
//    [MemberData(nameof(TestStrings))]
//    public void SpanByteWorks(string? testString)
//    { 
//        Span<byte> bytes = testString is not null ? Encoding.ASCII.GetBytes(testString) : [];
//
//        string? anyToString = Any.ToString<Span<byte>>(bytes);
//        Assert.NotNull(anyToString);
//
//        string? toString = bytes.ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//
//    [Fact]
//    public void RefStructWorks()
//    {
//        RefStructThing thing = new("test");
//
//        string? anyToString = Any.ToString(thing);
//        Assert.NotNull(anyToString);
//
//        string? toString = thing.ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//    
//    [Fact]
//    public void ReadonlyRefStructWorks()
//    {
//        ReadonlyRefStructThing thing = new("test");
//
//        string? anyToString = Any.ToString(thing);
//        Assert.NotNull(anyToString);
//
//        string? toString = thing.ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//    
//    [Fact]
//    public void EmptyRefStructWorks()
//    {
//        EmptyRefStructThing thing = new();
//
//        string? anyToString = Any.ToString(thing);
//        Assert.NotNull(anyToString);
//
//        // this will _not_ work
//        //string? toString = thing.ToString();
//
//        string? toString = typeof(EmptyRefStructThing).ToString();
//        Assert.NotNull(toString);
//        
//        Assert.Equal(toString, anyToString, StringComparer.Ordinal);
//    }
//#endif
//}