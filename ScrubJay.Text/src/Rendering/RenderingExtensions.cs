using Any = ScrubJay.Text.Rendering.Any;

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static partial class RenderingExtensions
{
    extension(Type)
    {
        public static string Render(Type type) => Renderer.RenderValue<Type>(type);

        public static string Render<T>() => Renderer.RenderValue<Type>(typeof(T));

        public static string Render<I>(in I? instance)
#if NET9_0_OR_GREATER
            where I : allows ref struct
#endif
            => Renderer.RenderValue<Type>(Any.GetType(in instance));
    }
}

public static class AnyRenderingExtensions
{
    extension(Any)
    {
        public static string Render<T>(in T? instance)
        {
            return Renderer.RenderValue<T>(instance);
        }

#if NET9_0_OR_GREATER
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static string Render<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
            where T : allows ref struct
        {
            return Renderer.RenderValue<T>(instance, _);
        }
#endif

        public static string Render<T>(scoped Span<T> span)
        {
            return Renderer.RenderSpan<T>(span);
        }

        public static string Render<T>(scoped ReadOnlySpan<T> span)
        {
            return Renderer.RenderSpan<T>(span);
        }
    }
}