// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Render<T>(T? value)
    {
        if (value is not null)
        {
            if (RendererCache.TryGetRenderer<T>(out var renderTo))
            {
                renderTo(value, this);
                return this;
            }
            else
            {
                return Append<T>(value);
            }
        }

        return Append("typeof(null)");
    }

#if NET9_0_OR_GREATER
    public TextBuilder Render<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is not null)
        {
            if (RendererCache.TryGetRenderer<T>(out var renderTo))
            {
                renderTo(value, this);
                return this;
            }
            else
            {
                return Append<T>(value, _);
            }
        }

        return Append("typeof(null)");
    }
#endif
}