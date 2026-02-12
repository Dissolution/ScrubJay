using System.Text.Json.Serialization;

namespace ScrubJay.Rendering;

/// <summary>
/// Indicates that this value can render itself to a <see cref="TextBuilder"/>.
/// </summary>
[PublicAPI]
public interface IRenderable
{
    /// <summary>
    /// Renders this instance to a <see cref="TextBuilder"/>.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns>
    /// A fluent reference back to the <see cref="TextBuilder"/> after rendering.
    /// </returns>
    TextBuilder RenderTo(TextBuilder builder);
}

[PublicAPI]
public abstract class RendererFactory
{
    public abstract Option<RenderTo<object>> TryGetRenderToFor(Type type);
    
    public abstract Option<RenderTo<T>> TryGetRenderToFor<T>();
}

public sealed class EnumRendererFactory : RendererFactory
{
    private readonly ConcurrentTypeMap<Delegate> _cache = [];
    
    public override Option<RenderTo<object>> TryGetRenderToFor(Type type)
    {
        throw new NotImplementedException();
    }
    
    public override Option<RenderTo<T>> TryGetRenderToFor<T>()
    {
        if (!typeof(T).IsEnum)
            return None;

        return (_cache.GetOrAdd<T>(CreateDelegate) as RenderTo<T>).IsNotNull();
    }

    private Delegate CreateDelegate(Type enumType)
    {
        
    }
    
    private RenderTo<E> CreateRenderTo<E>()
        where E : struct, Enum
    {
        return static (e, builder) =>
        {
            if (EnumTypeInfo.For<E>().TryGetMemberInfo(e).IsSome(out var info))
                return info.RenderTo(builder);
            return builder.Append(e.ToString());
        };
    }
}