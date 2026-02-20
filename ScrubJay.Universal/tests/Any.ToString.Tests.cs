namespace ScrubJay.Universal.Tests;

public class Any_ToString_Tests
{
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void CanToStringInt(int i32)
    {
        string str = Any.ToString<int>(i32);
        Assert.NotNull(str);
        Assert.Equal(i32.ToString(), str);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\0")]
    [InlineData("TRJ-147")]
    public void CanToStringString(string? str)
    {
        string anystr = Any.ToString<string>(str);
        Assert.NotNull(anystr);
        Assert.Equal(str ?? string.Empty, anystr);
    }

    [Theory]
    [InlineData('\0')]
    [InlineData(char.MaxValue)]
    [InlineData((char)0xD800)]
    public void CanToStringChar(char ch)
    {
        string str = Any.ToString<char>(ch);
        Assert.NotNull(str);
        Assert.True(str.Length == 1);
        Assert.Equal(ch, str[0]);
    }

    [Fact]
    public void CanToStringText()
    {
        ReadOnlySpan<char> text;
        string str;

        text = default;
        str = Any.ToString<ReadOnlySpan<char>>(text);
        Assert.NotNull(str);
        Assert.True(str.Length == 0);

        text = "TRJ".AsSpan();
        str = Any.ToString<ReadOnlySpan<char>>(text);
        Assert.NotNull(str);
        Assert.Equal("TRJ", str);
    }
}