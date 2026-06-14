namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class RenderingExtensions
{
    extension(Type)
    {
        public static string Render(Type? type) => Renderer.RenderType(type);

        public static string Render<T>() => Renderer.RenderType<T>();

        public static string Render<I>(in I? instance)
#if NET9_0_OR_GREATER
            where I : allows ref struct
#endif
            => Renderer.RenderType<I>(in instance);
    }
}

public static class AnyRenderingExtensions
{
    extension(Any)
    {
        public static string Render<T>(in T instance)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
            => Renderer.Render<T>(in instance);

        public static string Render(object? obj)
            => Renderer.RenderObject(obj);

        public static string Render<T>(scoped Span<T> span) => Renderer.RenderSpan<T>(span);

        public static string Render<T>(scoped ReadOnlySpan<T> span) => Renderer.RenderSpan<T>(span);
    }
}