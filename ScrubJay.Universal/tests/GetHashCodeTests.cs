using ScrubJay.Universal.Tests.Internal;

namespace ScrubJay.Universal.Tests;

public partial class GetHashCodeTests
{
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void CanGetHashCodeInt(int i32)
    {
        int hashcode = Any.GetHashCode<int>(in i32);

        Assert.Equal(
            i32.GetHashCode(),
            hashcode);
    }

    public static TheoryData<string?> StringData { get; } = new TheoryData<string?>(TestTypes.Text.StringsAndNull);

    [Theory]
    [MemberData(nameof(StringData))]
    public void CanGetHashCodeString(string? str)
    {
        int hashCode = str?.GetHashCode() ?? 0;
        int anyHashCode = Any.GetHashCode<string>(in str);

        Assert.Equal(hashCode, anyHashCode);
    }

    public static TheoryData<char> CharData { get; } = new(TestTypes.Text.Characters);

    [Theory]
    [MemberData(nameof(CharData))]
    public void CanGetHashCodeChar(char ch)
    {
        int hashCode = ch.GetHashCode();
        int anyHashCode = Any.GetHashCode<char>(in ch);

        Assert.Equal(hashCode, anyHashCode);
    }
    
    public static TheoryData<object?> ObjectData { get; } = new(TestTypes.Objects);
    
    [Theory]
    [MemberData(nameof(ObjectData))]
    public void CanHashCodeObject(object? obj)
    {
        int hashCode = obj?.GetHashCode() ?? 0;
        int anyHashCode = Any.GetHashCode<object>(in obj);
        
        Assert.Equal(hashCode, anyHashCode);
    }
}