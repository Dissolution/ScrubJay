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

    public TextBuilder RenderType<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return Render(typeof(T));
    }

    public TextBuilder RenderType(Type? type)
    {
        return Render(type);
    }

    public TextBuilder RenderTypeOf<I>(in I? instance)
    {
        return Render(Any.GetType<I>(in instance));
    }

#if NET9_0_OR_GREATER
    public TextBuilder RenderTypeOf<I>(in I? instance, TypeConstraints.AllowsRefStruct<I> _ = default)
    {
        return Render(Any.GetType<I>(in instance, _));
    }
#endif
}