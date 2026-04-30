namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class Renderer
{
    internal const char FORMAT = '@';


    public static void RenderValueTo<T>(T? value, TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is null)
            return;

        var action = RendererSingleton<T>.Action;
        if (action is not null)
        {
            action(value, builder);
        }
        else
        {
            RenderingManager.DefaultRenderTo<T>(value, builder);
        }
    }

    public static void RenderObjectTo(object? obj, TextBuilder builder)
    {
        if (obj is not null)
        {
            var action = RenderingManager.GetBoxedAction(obj.GetType());
            action(obj, builder);
        }
    }

    public static string RenderValue<T>(T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is null)
            return string.Empty;
        using var builder = TextBuilder.Rent();
        RenderValueTo<T>(value, builder);
        return builder.ToString();
    }

    public static string RenderObject(object? obj)
    {
        if (obj is null)
            return string.Empty;
        using var builder = TextBuilder.Rent();
        RenderObjectTo(obj, builder);
        return builder.ToString();
    }
}