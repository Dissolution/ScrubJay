namespace ScrubJay.Rendering.Rendition5;

[PublicAPI]
public interface IRenderable
{
    void RenderTo(TextBuilder builder);
}