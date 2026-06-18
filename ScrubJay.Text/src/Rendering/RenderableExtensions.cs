namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class RenderableExtensions
{
    extension<R>(R? renderable)
        where R : IRenderable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        public string Rendered()
        {
            if (renderable is null)
                return string.Empty;
            using var builder = TextBuilder.Rent();
            renderable.RenderTo(builder);
            return builder.ToString();
        }
    }
}