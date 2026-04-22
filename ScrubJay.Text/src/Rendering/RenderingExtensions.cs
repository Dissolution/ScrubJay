namespace ScrubJay.Text.Rendering;

/// <summary>
/// Extensions that add <c>Render</c> methods.
/// </summary>
[PublicAPI]
public static class RenderingExtensions
{
    /// <summary>
    /// Render this <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to render
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of value to be rendered.<br/>
    /// <b>Note:</b> in NET9.0+, this even allows for <c>ref struct</c> values.
    /// </typeparam>
    /// <returns>
    /// The <see cref="string"/> rendering.
    /// </returns>
    public static string Render<T>(this T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        using var builder = TextBuilder.Rent();
        builder.Render<T>(value);
        return builder.ToString();
    }

    public static string Render<T>(this ReadOnlySpan<T> span)
    {
        return TextBuilder.Rent()
            .Append('[')
            .Delimit(", ", span, TB<T>.Render)
            .Append(']')
            .ToStringAndDispose();

    }

    public static string Render<T>(this Span<T> span)
    {
        return TextBuilder.Rent()
            .Append('[')
            .Delimit(", ", span, TB.Render)
            .Append(']')
            .ToStringAndDispose();
    }
}