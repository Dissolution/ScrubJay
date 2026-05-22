// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Render<T>(T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Renderer.RenderTo<T>(value, this);
        return this;
    }

    public TextBuilder RenderType(Type? type)
    {
        Renderer.RenderTypeTo(type, this);
        return this;
    }

    public TextBuilder RenderType<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Renderer.RenderTypeTo<T>(this);
        return this;
    }

    public TextBuilder RenderTypeOf<T>(in T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Renderer.RenderTypeTo<T>(in instance, this);
        return this;
    }
}