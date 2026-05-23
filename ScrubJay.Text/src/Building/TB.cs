// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

/// <summary>
/// Cached delegates for common <see cref="TextBuilder"/> methods.
/// </summary>
[PublicAPI]
public static class TB
{
    public static Action<TextBuilder> None { get; } = static _ => { };

    public static Action<TextBuilder> NewLine { get; } = static tb => tb.NewLine();

#region Append
    public static Action<TextBuilder, T> Append<T>() => static (tb, value) => tb.Append<T>(value);

    public static Action<TextBuilder, T?> Append<T>(TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => static (tb, value) => tb.Append<T>(value);

    public static Action<TextBuilder> Append(char ch) => tb => tb.Append(ch);
    public static Action<TextBuilder> Append(string? str) => tb => tb.Append(str);
    public static Action<TextBuilder> Append<T>(T value) => tb => tb.Append<T>(value);

    public static void Append(TextBuilder builder, char ch) => builder.Append(ch);
    public static void Append(TextBuilder builder, scoped text text) => builder.Append(text);
    public static void Append(TextBuilder builder, string? str) => builder.Append(str);
    public static void Append<T>(TextBuilder builder, T? value) => builder.Append<T>(value);

    public static void Append<T>(TextBuilder builder, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.Append<T>(value);
#endregion

#region Render
    public static Action<TextBuilder, T?> Render<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => static (tb, value) => tb.Render<T>(value);

    public static Action<TextBuilder> Render<T>(T? value)
        => tb => tb.Render<T>(value);

    public static void Render<T>(TextBuilder builder, T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.Render<T>(value);
#endregion

#region Format
    public static Action<TextBuilder, T?> Format<T>() => static (tb, value) => tb.Format<T>(value);
    public static Action<TextBuilder, T?> Format<T>(string? format) => (tb, value) => tb.Format<T>(value, format);
    public static Action<TextBuilder, T?> Format<T>(string? format, IFormatProvider? provider) => (tb, value) => tb.Format<T>(value, format, provider);

    public static Action<TextBuilder, T?> Format<T>(TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => static (tb, value) => tb.Format<T>(value);

    public static Action<TextBuilder, T?> Format<T>(string? format, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => (tb, value) => tb.Format<T>(value, format, null);

    public static Action<TextBuilder, T?> Format<T>(string? format, IFormatProvider? provider, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => (tb, value) => tb.Format<T>(value, format, provider);

    public static Action<TextBuilder> Format<T>(T? value)
        => tb => tb.Format<T>(value);

    public static Action<TextBuilder> Format<T>(T? value, string? format)
        => tb => tb.Format<T>(value, format);

    public static Action<TextBuilder> Format<T>(T? value, string? format, IFormatProvider? provider)
        => tb => tb.Format<T>(value, format, provider);


    public static void Format<T>(TextBuilder builder, T? value) => builder.Format<T>(value);
    public static void Format<T>(TextBuilder builder, T? value, string? format) => builder.Format<T>(value, format);
    public static void Format<T>(TextBuilder builder, T? value, string? format, IFormatProvider? provider) => builder.Format<T>(value, format, provider);


    public static void Format<T>(TextBuilder builder, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.Format<T>(value, null, null);

    public static void Format<T>(TextBuilder builder, T? value, string? format, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.Format<T>(value, format, null);

    public static void Format<T>(TextBuilder builder, T? value, string? format, IFormatProvider? provider, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.Format<T>(value, format, provider);
#endregion
}

/// <summary>
/// Cached delegates for common <see cref="TextBuilder"/> methods.
/// </summary>
[PublicAPI]
public static class TB<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static readonly Action<TextBuilder, T> Append = static (tb, value) => tb.Append<T>(value);
    public static readonly Action<TextBuilder, T> Render = static (tb, value) => tb.Render<T>(value);
    public static readonly Action<TextBuilder, T> Format = static (tb, value) => tb.Format<T>(value);
}