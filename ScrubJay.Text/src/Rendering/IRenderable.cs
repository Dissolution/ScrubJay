namespace ScrubJay.Text.Rendering;

[PublicAPI]
public interface IRenderable
{
    void RenderTo(TextBuilder builder);
}