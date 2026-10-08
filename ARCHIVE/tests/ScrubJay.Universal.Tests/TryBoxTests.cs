//using ScrubJay.Universal.Tests.Internal;
//
//namespace ScrubJay.Universal.Tests;
//
//public class Any_Boxing_Tests
//{
//    [Theory]
//    [InlineData(int.MinValue)]
//    [InlineData(-1)]
//    [InlineData(0)]
//    [InlineData(1)]
//    [InlineData(int.MaxValue)]
//    public void CanBoxInt(int i32)
//    {
//        var boxed = Any.TryBox(
//            i32,
//            out object? box);
//
//        Assert.True(boxed);
//        Assert.NotNull(box);
//
//        Assert.Equal(
//            typeof(int),
//            box.GetType());
//
//        Assert.True(box.Equals(i32));
//        Assert.True(i32.Equals(box));
//    }
//
//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("\0")]
//    [InlineData("TRJ-147")]
//    public void CanBoxString(string? str)
//    {
//        var boxed = Any.TryBox(
//            str,
//            out object? box);
//
//        Assert.True(boxed);
//        Assert.True((box is null) == (str is null));
//
//        if (str is not null)
//        {
//            Assert.Equal(
//                typeof(string),
//                box!.GetType());
//
//            Assert.True(box.Equals(str));
//            Assert.True(str.Equals(box));
//        }
//    }
//
//    [Theory]
//    [InlineData('\0')]
//    [InlineData(char.MaxValue)]
//    [InlineData((char)0xD800)]
//    public void CanBoxChar(char ch)
//    {
//        var boxed = Any.TryBox(
//            ch,
//            out object? box);
//
//        Assert.True(boxed);
//        Assert.NotNull(box);
//
//        Assert.Equal(
//            typeof(char),
//            box.GetType());
//
//        Assert.True(box.Equals(ch));
//        Assert.True(ch.Equals(box));
//    }
//
//    public static TheoryData<object?> ObjectData { get; } = new(TestTypes.Objects);
//
//    [Theory]
//    [MemberData(nameof(ObjectData))]
//    public void CanBoxObject(object? obj)
//    {
//        bool didBox = Any.TryBox(obj, out object? boxed);
//        Assert.True(didBox);
//        if (obj is not null)
//        {
//            Assert.NotNull(boxed);
//            Assert.Equal(obj, boxed);
//        }
//        else
//        {
//            Assert.Null(boxed);
//        }
//    }
//
//#if NET9_0_OR_GREATER
//    [Fact]
//    public void CannotBoxText()
//    {
//        ReadOnlySpan<char> text;
//        bool boxed;
//        object? box;
//
//        text = default;
//
//        boxed = Any.TryBox<ReadOnlySpan<char>>(
//            text,
//            out box);
//
//        Assert.False(boxed);
//        Assert.Null(box);
//
//        text = "TRJ".AsSpan();
//
//        boxed = Any.TryBox<ReadOnlySpan<char>>(
//            text,
//            out box);
//
//        Assert.False(boxed);
//        Assert.Null(box);
//
//        string str = "TRJ";
//        text = str;
//
//        boxed = Any.TryBox<ReadOnlySpan<char>>(
//            text,
//            out box);
//
//        Assert.False(boxed);
//        Assert.Null(box);
//    }
//#endif
//}