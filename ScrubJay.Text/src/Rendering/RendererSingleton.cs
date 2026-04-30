using System.Reflection;

namespace ScrubJay.Text.Rendering;

internal static class RendererSingleton<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    internal static readonly Action<T, TextBuilder>? Action;

    static RendererSingleton()
    {
        if (typeof(T) != typeof(object))
        {
            Action = RenderingManager.GetSingletonAction<T>();
        }
        else
        {
            var renderObjectToMethod = typeof(Renderer)
                .GetMethod(nameof(Renderer.RenderObjectTo), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!;

            Action = (Action<T, TextBuilder>)Delegate.CreateDelegate(typeof(Action<T, TextBuilder>), renderObjectToMethod);
        }
    }
}