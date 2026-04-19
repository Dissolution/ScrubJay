// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Render<T>(T? value)
    {
        throw new NotImplementedException();
    }

#if NET9_0_OR_GREATER
    public TextBuilder Render<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        throw new NotImplementedException();
    }
#endif
}