using UNPROCESSED_Any = ScrubJay.Universal.UNPROCESSED.Any;

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static partial class RenderingExtensions
{
    extension(Type)
    {
        public static string Render(Type type) => Renderer.RenderValue<Type>(type);

        public static string Render<T>() => Renderer.RenderValue<Type>(typeof(T));

        public static string Render<I>(in I? instance) => Renderer.RenderValue<Type>(UNPROCESSED_Any.GetType(in instance));

#if NET9_0_OR_GREATER
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static string Render<I>(in I? instance, TypeConstraints.AllowsRefStruct<I> _ = default)
            where I : allows ref struct
            => Renderer.RenderValue<Type>(UNPROCESSED_Any.GetType(in instance, _));
#endif
    }
}

public static class AnyRenderingExtensions
{
    extension(Universal.UNPROCESSED.Any)
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