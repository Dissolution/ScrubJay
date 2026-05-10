using static ScrubJay.Universal.Tests.Internal.TestTypes.OverrideToString;

namespace ScrubJay.Universal.Tests;

public class ToStringTests_Custom_OverriddenToString
{
    [Fact]
    public void Any_ToString_TestStruct_Works()
    {
        TestStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyStruct_Works()
    {
        TestReadonlyStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void Any_ToString_TestRefStruct_Works()
    {
        TestRefStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyRefStruct_Works()
    {
        TestReadonlyRefStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }
#endif

    [Fact]
    public void Any_ToString_TestRecordStruct_Works()
    {
        TestRecordStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestReadonlyRecordStruct_Works()
    {
        TestReadonlyRecordStruct instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestClass_Works()
    {
        TestClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestSealedClass_Works()
    {
        TestSealedClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestAbstractClass_Works()
    {
        TestAbstractClass instance = new TestParentClass();

        // directly on the abstract type
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestParentClass_Works()
    {
        TestParentClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestGrandParentClass_Works()
    {
        TestGrandParentClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestRecordClass_Works()
    {
        TestRecordClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }

    [Fact]
    public void Any_ToString_TestSealedRecordClass_Works()
    {
        TestSealedRecordClass instance = new();
        string? anyStr = UNPROCESSED.Any.ToString(in instance);
        Assert.NotNull(anyStr);
        string? str = instance.ToString();
        Assert.Equal(str, anyStr);
    }


}