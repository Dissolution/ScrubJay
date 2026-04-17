//#pragma warning disable CA1815, IDE0250, CA1001
//
//namespace ScrubJay.Text.Building;
//
///// <summary>
///// Provides a handler used to append interpolated strings into <see cref="TextBuilder"/> instances.
///// </summary>
///// <remarks>
///// Heavily inspired by <see cref="DefaultInterpolatedStringHandler"/> and System.Text.AppendInterpolatedStringHandler
///// </remarks>
//[PublicAPI]
//#if !NETFRAMEWORK && !NETSTANDARD
//[InterpolatedStringHandler]
//#endif
//[MustDisposeResource(false)]
//public ref struct InterpolatedTextBuilder
//{
//    internal TextBuilder _textBuilder;
//
//    public readonly int Length => _textBuilder.Length;
//
//    public InterpolatedTextBuilder() => new InvalidOperationException();
//    public InterpolatedTextBuilder(int literalLength, int formattedCount)=> new InvalidOperationException();
//
//    public InterpolatedTextBuilder(int literalLength, int formattedCount, TextBuilder builder)
//    {
//        _textBuilder = builder;
//    }
//
//    public void AppendLiteral(string str)
//        => _textBuilder.Write(str);
//
//    public void AppendFormatted(in char ch)
//        => _textBuilder.Write(in ch);
//
////    public void AppendFormatted(char ch, int alignment)
////        => _textBuilder.Align(ch, alignment);
//
//    public void AppendFormatted(string? str)
//        => _textBuilder.Write(str);
//
////    public void AppendFormatted(string? str, int alignment)
////        => _textBuilder.Align(str, alignment);
//
//    public void AppendFormatted(scoped text text)
//        => _textBuilder.Write(text);
//
////    public void AppendFormatted(scoped text text, int alignment)
////        => _textBuilder.Align(text, alignment);
//
//    public void AppendFormatted<T>(T? value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => _textBuilder.Write(value);
//
////    public void AppendFormatted<T>(T? value, scoped text format)
////#if NET9_0_OR_GREATER
////        where T : allows ref struct
////#endif
////        => _textBuilder.Format<T>(value, format);
////
////    public void AppendFormatted<T>(T? value, string? format)
////        => _textBuilder.Format<T>(value, format);
//
//    public override string ToString()
//        => _textBuilder.ToString();
//}