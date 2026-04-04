using ScrubJay.Universal.Tests.Internal;

namespace ScrubJay.Universal.Tests;

public class Any_GetHashCode_Tests
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
        int hashcode = Any.GetHashCode<char>(in ch);

        Assert.Equal(
            ch.GetHashCode(),
            hashcode);
    }
}