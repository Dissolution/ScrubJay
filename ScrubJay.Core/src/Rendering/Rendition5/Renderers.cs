namespace ScrubJay.Rendering.Rendition5;

[PublicAPI]
public static partial class Renderers
{
    [RenderToMethod]
    public static void RenderRenderableTo<R>(R renderable, TextBuilder builder)
        where R : IRenderable
    {
        renderable.RenderTo(builder);
    }

    [RenderToMethod]
    public static void RenderEnumTo<E>(E @enum, TextBuilder builder)
        where E : struct, Enum
    {
        if (EnumTypeInfo.For<E>(@enum).TryGetMemberInfo(@enum).IsSome(out var memberInfo))
        {
            memberInfo.RenderTo(builder);
        }
        else
        {
            builder.Append(@enum);
        }
    }
}