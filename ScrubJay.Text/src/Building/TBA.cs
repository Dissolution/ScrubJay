// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

public delegate void BuildWithReadOnlySpan<T>(TextBuilder builder, ReadOnlySpan<T> span);

public static class TBA
{
#region Action<TextBuilder>
    public static void None(TextBuilder _) { }
    public static Action<TextBuilder> None() => static _ => { };

    public static void NewLine(TextBuilder builder) => builder.NewLine();
    public static Action<TextBuilder> NewLine() => static tb => tb.NewLine();
#endregion

#region Action<TextBuilder, char>
    public static Action<TextBuilder> Append(char ch) => tb => tb.Append(ch);
    public static void Append(TextBuilder builder, char ch) => builder.Append(ch);
#endregion

#region Action<TextBuilder, string?>
    public static Action<TextBuilder> Append(string? str) => tb => tb.Append(str);
    public static void Append(TextBuilder builder, string? str) => builder.Append(str);
#endregion

#region Action<TextBuilder, T?>
    public static Action<TextBuilder> Append<T>(T? value) => tb => tb.Append<T>(value);
    public static Action<TextBuilder, T?> Append<T>() => static (tb, value) => tb.Append<T>(value);
    public static void Append<T>(TextBuilder builder, T? value) => builder.Append<T>(value);

#if NET9_0_OR_GREATER
    public static Action<TextBuilder, T?> Append<T>(TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => static (tb, value) => tb.Append<T>(value, default);

    public static void Append<T>(TextBuilder builder, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => builder.Append<T>(value, _);
#endif


    public static Action<TextBuilder> Render<T>(T? value) => tb => tb.Render<T>(value);
    public static Action<TextBuilder, T?> Render<T>() => static (tb, value) => tb.Render<T>(value);
    public static void Render<T>(TextBuilder builder, T? value) => builder.Render<T>(value);

#if NET9_0_OR_GREATER
    public static Action<TextBuilder, T?> Render<T>(TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => static (tb, value) => tb.Render<T>(value, default);

    public static void Render<T>(TextBuilder builder, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => builder.Render<T>(value, _);
#endif


#region Format
    public static Action<TextBuilder> Format<T>(T? value)
        => tb => tb.Format<T>(value);

    public static Action<TextBuilder> Format<T>(T? value, string? format)
        => tb => tb.Format<T>(value, format);

    public static Action<TextBuilder> Format<T>(T? value, string? format, IFormatProvider? provider)
        => tb => tb.Format<T>(value, format, provider);


    public static Action<TextBuilder, T?> Format<T>()
        => static (tb, value) => tb.Format<T>(value);

    public static Action<TextBuilder, T?> Format<T>(string? format)
        => (tb, value) => tb.Format<T>(value, format);

    public static Action<TextBuilder, T?> Format<T>(string? format, IFormatProvider? provider)
        => (tb, value) => tb.Format<T>(value, format, provider);

#if NET9_0_OR_GREATER
    public static Action<TextBuilder, T?> Format<T>(TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => static (tb, value) => tb.Format<T>(value);

    public static Action<TextBuilder, T?> Format<T>(string? format, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => (tb, value) => tb.Format<T>(value, format, null, _);

    public static Action<TextBuilder, T?> Format<T>(string? format, IFormatProvider? provider, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => (tb, value) => tb.Format<T>(value, format, provider, _);
#endif


    public static void Format<T>(TextBuilder builder, T? value)
        => builder.Format<T>(value);

    public static void Format<T>(TextBuilder builder, T? value, string? format)
        => builder.Format<T>(value, format);

    public static void Format<T>(TextBuilder builder, T? value, string? format, IFormatProvider? provider)
        => builder.Format<T>(value, format, provider);

#if NET9_0_OR_GREATER
    public static void Format<T>(TextBuilder builder, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => builder.Format<T>(value, null, null, _);

    public static void Format<T>(TextBuilder builder, T? value, string? format, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => builder.Format<T>(value, format, null, _);

    public static void Format<T>(TextBuilder builder, T? value, string? format, IFormatProvider? provider, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => builder.Format<T>(value, format, provider, _);
#endif
#endregion
#endregion

}

public static class TBA<T>
{
#region Action<TextBuilder, T?>
    public static Action<TextBuilder> Append(T? value) => tb => tb.Append<T>(value);
    public static Action<TextBuilder, T?> Append() => static (tb, value) => tb.Append<T>(value);
    public static void Append(TextBuilder builder, T? value) => builder.Append<T>(value);

    public static Action<TextBuilder> Render(T? value) => tb => tb.Render<T>(value);
    public static Action<TextBuilder, T?> Render() => static (tb, value) => tb.Render<T>(value);
    public static void Render(TextBuilder builder, T? value) => builder.Render<T>(value);

#region Format
    public static Action<TextBuilder> Format(T? value)
        => tb => tb.Format<T>(value);

    public static Action<TextBuilder> Format(T? value, string? format)
        => tb => tb.Format<T>(value, format);

    public static Action<TextBuilder> Format(T? value, string? format, IFormatProvider? provider)
        => tb => tb.Format<T>(value, format, provider);


    public static Action<TextBuilder, T?> Format()
        => static (tb, value) => tb.Format<T>(value);

    public static Action<TextBuilder, T?> Format(string? format)
        => (tb, value) => tb.Format<T>(value, format);

    public static Action<TextBuilder, T?> Format(string? format, IFormatProvider? provider)
        => (tb, value) => tb.Format<T>(value, format, provider);


    public static void Format(TextBuilder builder, T? value)
        => builder.Format<T>(value);

    public static void Format(TextBuilder builder, T? value, string? format)
        => builder.Format<T>(value, format);

    public static void Format(TextBuilder builder, T? value, string? format, IFormatProvider? provider)
        => builder.Format<T>(value, format, provider);
#endregion
#endregion
}