namespace ScrubJay.Text.Rendering;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class RenderingMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class RenderAsAttribute : Attribute
{
    public required string Render { get; init; }

    public RenderAsAttribute()
    {
    }

    [SetsRequiredMembers]
    public RenderAsAttribute(string render)
    {
        Render = render;
    }
}

internal static class RenderingCache<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static Action<T, TextBuilder>? Delegate;
}