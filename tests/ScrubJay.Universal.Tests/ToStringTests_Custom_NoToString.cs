//using static ScrubJay.Universal.Tests.Internal.TestTypes.NoToString;
//
//namespace ScrubJay.Universal.Tests;
//
//public class ToStringTests_Custom_NoToString
//{
//  
//#if NET9_0_OR_GREATER
//    [Fact]
//    public void CanToStringText()
//    {
//        ReadOnlySpan<char> text;
//        string str;
//
//        text = default;
//        str = Any.ToString<ReadOnlySpan<char>>(in text);
//        Assert.NotNull(str);
//        Assert.True(str.Length == 0);
//
//        text = "TRJ".AsSpan();
//        str = Any.ToString<ReadOnlySpan<char>>(in text);
//        Assert.NotNull(str);
//        Assert.Equal("TRJ", str);
//    }
//#endif
//
//    [Fact]
//    public void Any_ToString_TestEnum_Works()
//    {
//        TestEnum instance = TestEnum.Bravo;
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestFlaggedEnum_Works()
//    {
//        TestFlaggedEnum instance = TestFlaggedEnum.Gamma | TestFlaggedEnum.Delta;
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestStruct_Works()
//    {
//        TestStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestReadonlyStruct_Works()
//    {
//        TestReadonlyStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//#if NET9_0_OR_GREATER
//    [Fact]
//    public void Any_ToString_TestRefStruct_Works()
//    {
//        TestRefStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = typeof(TestRefStruct).ToString();
//        //string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestReadonlyRefStruct_Works()
//    {
//        TestReadonlyRefStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = typeof(TestReadonlyRefStruct).ToString();
//        //string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//#endif
//
//    [Fact]
//    public void Any_ToString_TestRecordStruct_Works()
//    {
//        TestRecordStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestReadonlyRecordStruct_Works()
//    {
//        TestReadonlyRecordStruct instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestClass_Works()
//    {
//        TestClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestSealedClass_Works()
//    {
//        TestSealedClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestAbstractClass_Works()
//    {
//        TestAbstractClass instance = new TestParentClass();
//
//        // directly on the abstract type
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestParentClass_Works()
//    {
//        TestParentClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestGrandParentClass_Works()
//    {
//        TestGrandParentClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestRecordClass_Works()
//    {
//        TestRecordClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//    [Fact]
//    public void Any_ToString_TestSealedRecordClass_Works()
//    {
//        TestSealedRecordClass instance = new();
//        string? anyStr = Any.ToString(in instance);
//        Assert.NotNull(anyStr);
//        string? str = instance.ToString();
//        Assert.Equal(str, anyStr);
//    }
//
//
//}