namespace ScrubJay.Text.Building;

[PublicAPI]
public delegate void TextBuildWithReadOnlySpan<T>(TextBuilder builder, scoped ReadOnlySpan<T> span);

[PublicAPI]
public delegate void ReadOnlySpanTextBuild<T>(scoped ReadOnlySpan<T> span, TextBuilder builder);

[PublicAPI]
public delegate void TextBuildWithValue<in T>(TextBuilder builder, T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate void ValueTextBuild<in T>(T value, TextBuilder builder)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate void TextBuildWithInValue<T>(TextBuilder builder, in T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate void InValueTextBuild<T>(in T value, TextBuilder builder)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;