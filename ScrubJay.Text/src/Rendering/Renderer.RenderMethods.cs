// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static partial class Renderer
{
    /// <summary>
    /// If used in a Format method, this indicates that Rendering should be used.
    /// </summary>
    public const char FORMAT = '@';

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

    internal static void DefaultRenderTo<T>(T value, TextBuilder builder)
    {
        builder.Append<T>(value);
    }

    public static void RenderValueTo<T>(T? value, TextBuilder builder)
    {
        if (value is not null)
        {
            var action = Cache<T>.Action;
            if (action is not null)
            {
                action(value, builder);
                return;
            }

            DefaultRenderTo<T>(value, builder);
            return;
        }

        RenderNullTo<T>(builder);
    }

    public static string RenderValue<T>(in T? value)
    {
        using var builder = TextBuilder.Rent();
        RenderValueTo<T>(value, builder);
        return builder.ToString();
    }


#if NET9_0_OR_GREATER
    internal static void DefaultRenderTo<T>(T value, TextBuilder builder, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        builder.Append<T>(value, _);
    }

    public static void RenderValueTo<T>(T? value, TextBuilder builder, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is not null)
        {
            var action = Cache<T>.Action;
            if (action is not null)
            {
                action(value, builder);
                return;
            }

            DefaultRenderTo<T>(value, builder, _);
            return;
        }

        RenderNullTo<T>(builder);
    }

    public static string RenderValue<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        using var builder = TextBuilder.Rent();
        RenderValueTo<T>(value, builder, _);
        return builder.ToString();
    }
#endif



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