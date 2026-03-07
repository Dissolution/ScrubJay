namespace ScrubJay.Rendering.Rendition5;

[PublicAPI]
public interface IRenderable
{
    void RenderTo(TextBuilder builder);
}

public static class RenderingExtensions
{
    extension(IRenderable)
    {
        public static string ToString<R>(R? renderable)
            where R : IRenderable
        {
            if (renderable is null)
                return "null";

            return TextBuilder.Build<R>(renderable, static (builder, renderable) => renderable.RenderTo(builder));
        }
    }
}