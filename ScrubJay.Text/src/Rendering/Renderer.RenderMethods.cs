// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static partial class Renderer
{
    /// <summary>
    /// If used in a Format method, this indicates that Rendering should be used.
    /// </summary>
    public const char FORMAT = '@';



    public static void RenderObjectTo(object? obj, TextBuilder builder)
    {
        if (obj is not null)
        {
            Type objType = obj.GetType();
            var action = GetObjectRenderTo(objType);
            action(obj, builder);
            return;
        }

        RenderNullTo<object?>(builder);
    }

    public static string RenderObject(object? obj)
    {
        if (obj is null)
            return string.Empty;
        using var builder = TextBuilder.Rent();
        RenderObjectTo(obj, builder);
        return builder.ToString();
    }

    public static string RenderType<T>()
    {
        using var builder = TextBuilder.Rent();
        TypeRenderer.RenderTypeTo(typeof(T), builder);
        return builder.ToString();
    }

    public static string RenderType(Type? type)
    {
        using var builder = TextBuilder.Rent();
        TypeRenderer.RenderTypeTo(type, builder);
        return builder.ToString();
    }

    public static string RenderType<T>(T? _)
    {
        using var builder = TextBuilder.Rent();
        TypeRenderer.RenderTypeTo(typeof(T), builder);
        return builder.ToString();
    }

    public static string RenderSpan<T>(scoped Span<T> span)
    {
        using var builder = TextBuilder.Rent();
        Renderers.RenderSpanTo(span, builder);
        return builder.ToString();
    }

    public static string RenderSpan<T>(scoped ReadOnlySpan<T> span)
    {
        using var builder = TextBuilder.Rent();
        Renderers.RenderReadOnlySpanTo(span, builder);
        return builder.ToString();
    }
}