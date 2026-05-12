using static ScrubJay.Universal.Tests.Internal.TestTypes.NoToString;

namespace ScrubJay.Universal.Tests;

public class ToStringTests_Custom_NoToString
{
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void CanToStringInt(int i32)
    {
        string str = Any.ToString<int>(in i32);
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
        string? anystr = Any.ToString<string>(in str);
        Assert.True((anystr is null) == (str is null));
        Assert.Equal(str, anystr);
    }

    [Theory]
    [InlineData('\0')]
    [InlineData(char.MaxValue)]
    [InlineData((char)0xD800)]
    public void CanToStringChar(char ch)
    {
        string str = Any.ToString<char>(in ch);
        Assert.NotNull(str);
        Assert.True(str.Length == 1);
        Assert.Equal(ch, str[0]);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void CanToStringText()
    {
        ReadOnlySpan<char> text;
        string str;

        text = default;
        str = Any.ToString<ReadOnlySpan<char>>(in text);
        Assert.NotNull(str);
        Assert.True(str.Length == 0);

        text = "TRJ".AsSpan();
        str = Any.ToString<ReadOnlySpan<char>>(in text);
        Assert.NotNull(str);
        Assert.Equal("TRJ", str);
    }
#endif

    [Fact]
    public void Any_ToString_TestEnum_Works()
    {
        TestEnum instance = TestEnum.Bravo;
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestFlaggedEnum_Works()
    {
        TestFlaggedEnum instance = TestFlaggedEnum.Gamma | TestFlaggedEnum.Delta;
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestStruct_Works()
    {
        TestStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyStruct_Works()
    {
        TestReadonlyStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void Any_ToString_TestRefStruct_Works()
    {
        TestRefStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = typeof(TestRefStruct).ToString();
        //string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyRefStruct_Works()
    {
        TestReadonlyRefStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = typeof(TestReadonlyRefStruct).ToString();
        //string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }
#endif

    [Fact]
    public void Any_ToString_TestRecordStruct_Works()
    {
        TestRecordStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyRecordStruct_Works()
    {
        TestReadonlyRecordStruct instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestClass_Works()
    {
        TestClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestSealedClass_Works()
    {
        TestSealedClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestAbstractClass_Works()
    {
        TestAbstractClass instance = new TestParentClass();

        // directly on the abstract type
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestParentClass_Works()
    {
        TestParentClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestGrandParentClass_Works()
    {
        TestGrandParentClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestRecordClass_Works()
    {
        TestRecordClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestSealedRecordClass_Works()
    {
        TestSealedRecordClass instance = new();
        string? anyStr = Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }


}