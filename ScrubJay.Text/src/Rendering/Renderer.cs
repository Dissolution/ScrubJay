// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class Renderer
{
    /// <summary>
    /// If used in a Format method, this indicates that Rendering should be used.
    /// </summary>
    public const char FORMAT = '@';

    #region Render To
    internal static void RenderNullTo<T>(TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        builder
            .Append('(')
            .RenderType<T>()
            .Append(")null");
    }

    public static void RenderTo<T>(in T? instance, TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
        {
            RenderNullTo<T>(builder);
        }
        else
        {
            RenderToCache<T>.Invoke(in instance, builder);
        }
    }

    public static void RenderObjectTo(object? obj, TextBuilder builder)
    {
        if (obj is null)
        {
            RenderNullTo<object?>(builder);
        }
        else
        {
            Type objType = obj.GetType();
            var action = RenderToCache.GetObjectRenderTo(objType);
            action(obj, builder);
        }
    }

    public static void RenderTypeTo(Type? type, TextBuilder builder)
    {
        TypeRenderer.RenderTypeTo(type, builder);
    }

    public static void RenderTypeTo<T>(TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        TypeRenderer.RenderTypeTo(typeof(T), builder);
    }

    public static void RenderTypeTo<T>(in T? instance, TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        TypeRenderer.RenderTypeTo(Any.GetType<T>(in instance), builder);
    }

    public static void RenderSpanTo<T>(scoped ReadOnlySpan<T> span, TextBuilder builder)
    {
        CollectionRenderers.RenderReadOnlySpanTo(span, builder);
    }

    public static void RenderSpanTo<T>(scoped Span<T> span, TextBuilder builder)
    {
        CollectionRenderers.RenderSpanTo(span, builder);
    }
    #endregion

    public static string Render<T>(in T instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TextBuilder.Build<T>(in instance, RenderTo<T>);
    }

    public static string RenderObject(object? obj)
    {
        return TextBuilder.Build(obj, RenderObjectTo);
    }

    public static string RenderType(Type? type)
    {
        return TextBuilder.Build(type, RenderTypeTo);
    }

    public static string RenderType<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TextBuilder.Build(RenderTypeTo<T>);
    }

    public static string RenderType<T>(in T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TextBuilder.Build(in instance, RenderTypeTo<T>);
    }

    public static string RenderSpan<T>(scoped Span<T> span)
    {
        return TextBuilder.Build<T>(span, RenderSpanTo);
    }

    public static string RenderSpan<T>(scoped ReadOnlySpan<T> span)
    {
        return TextBuilder.Build<T>(span, RenderSpanTo);
    }

}