// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Render<T>(T? value)
    {
        Renderer.RenderValueTo<T>(value, this);
        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Render<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        Renderer.RenderValueTo<T>(value, this);
        return this;
    }
#endif
}