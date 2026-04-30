namespace ScrubJay.Text.Rendering;

[PublicAPI]
public interface IRenderer<in T>
{
#if NET7_0_OR_GREATER
    [RenderToMethod]
    abstract static void RenderValueTo(T value, TextBuilder builder);
#endif
}