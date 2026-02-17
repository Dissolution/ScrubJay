namespace ScrubJay.Rendering.Rendition3;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class RendersAttribute : Attribute
{
    public required Type Type { get; init; }

    public RendersAttribute() { }

    [SetsRequiredMembers]
    public RendersAttribute(Type type)
    {
        this.Type = type;
    }
}

/// <summary>
/// When applied to a non-generic method with the same signature as <see cref="RenderTo{T}"/>,
/// indicates that method will be used for Rendering that type.
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of value this method will be used to render.<br/>
/// <b>WARNING</b>: Runtime exceptions will be thrown if this <see cref="Type"/> does not match the type of the first parameter in the method.
/// </typeparam>
/// <remarks>
/// Example:
/// <code>
/// [Renders&lt;char&gt;]
/// public static TextBuilder RenderTo(this char ch, TextBuilder builder)
/// { ... }
/// </code>
/// </remarks>
[PublicAPI]
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class RendersAttribute<T> : RendersAttribute
{
    [SetsRequiredMembers]
    public RendersAttribute() : base(typeof(T)) { }
}