namespace ScrubJay.Text.Rendering;

public static partial class Renderer
{

    internal static class Cache<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    {
        public static readonly Action<T, TextBuilder>? Action;

        static Cache()
        {
            if (typeof(T) != typeof(object))
            {
                Action = GetRenderToForCache<T>();
                return;
            }
        }
        
        public static void DefaultRenderTo(in T instance, TextBuilder builder)
        {
            builder.Append(Any.ToString(in instance));
        }
    }
}