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
        int hashcode = Any.GetHashCode<int>(i32);

        Assert.Equal(
            i32.GetHashCode(),
            hashcode);
    }

#pragma warning disable CA1307, MA0021
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\0")]
    [InlineData("TRJ-147")]
    public void CanGetHashCodeString(string? str)
    {
        int hashcode = Any.GetHashCode<string>(str);

        Assert.Equal(
            str?.GetHashCode() ?? 0,
            hashcode);
    }
#pragma warning restore CA1307, MA0021

    [Theory]
    [InlineData('\0')]
    [InlineData(char.MaxValue)]
    [InlineData((char)0xD800)]
    public void CanGetHashCodeChar(char ch)
    {
        int hashcode = Any.GetHashCode<char>(ch);

        Assert.Equal(
            ch.GetHashCode(),
            hashcode);
    }

    [Fact]
    public void ReadOnlySpanGetHashCodeDoesNotThrow()
    {
        ReadOnlySpan<int> ros =
        [
            1,
            4,
            7
        ];

        int hashcode;

        try
        {
            hashcode = ros.GetHashCode();
        }
        catch (Exception ex)
        {
            Assert.IsType<NotSupportedException>(ex);
        }

        hashcode = Any.GetHashCode(ros);
        Assert.True(hashcode == 0);
    }
}