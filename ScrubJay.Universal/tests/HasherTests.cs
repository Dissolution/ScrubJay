using ScrubJay.Universal.Tests.Internal;

namespace ScrubJay.Universal.Tests;

public class HasherTests
{
    [Fact]
    public void NullHashesToNullHash()
    {
        var nullhash = Hasher.NullHash;
        var a = Hasher.Hash<object?>(null);
        Assert.Equal(nullhash, a);
        var b = Hasher.Hash<int?>(null);
        Assert.Equal(nullhash, b);
        var c = Hasher.Hash<string>(null);
        Assert.Equal(nullhash, c);
    }

    [Fact]
    public void EmptyHashesToEmptyHash()
    {
        var emptyhash = Hasher.EmptyHash;

        // array
        var a = Hasher.HashMany<int>(array: Array.Empty<int>());
        Assert.Equal(emptyhash, a);

        // span
        var b = Hasher.HashMany<string?>(span: ReadOnlySpan<string>.Empty);
        Assert.Equal(emptyhash, b);

        // enumerable
        var c = Hasher.HashMany<object?>(enumerable: new List<object?>(0));
        Assert.Equal(emptyhash, c);

        var d = Hasher.HashMany<char>(string.Empty);
        Assert.Equal(emptyhash, d);
    }

    public static TheoryData<object?> ObjectData { get; } = new(TestTypes.Objects);


    [Theory]
    [MemberData(nameof(ObjectData))]
    public void HashBytesObjectWorks(object? obj)
    {
        // We just want to be sure this works
        var hash = Hasher.HashBytes(in obj);
        Assert.NotEqual(0, hash);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void HashBytesRefStructWorks()
    {
        Span<int> span = [2, 3, 13];
        var hash = Hasher.HashBytes(in span);
        Assert.NotEqual(0, hash);
    }
#endif
}