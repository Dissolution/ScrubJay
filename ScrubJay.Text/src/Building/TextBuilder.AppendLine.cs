namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder AppendLine(char ch) => Append(ch).NewLine();

    public TextBuilder AppendLine(scoped text text) => Append(text).NewLine();

    public TextBuilder AppendLine(string? str) => Append(str).NewLine();

    public TextBuilder AppendLine(
        [InterpolatedStringHandlerArgument("")]
        ref InterpolatedTextBuilder interpolatedTextBuilder)
        => Append(ref interpolatedTextBuilder).NewLine();

    public TextBuilder AppendLine<T>(T? value)
        => Append<T>(value).NewLine();

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public TextBuilder AppendLine<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => Append<T>(value).NewLine();
#endif

}