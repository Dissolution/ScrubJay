namespace ScrubJay.Text.Rendering;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public sealed class RenderToMethodAttribute : Attribute
{
    //public bool AcceptsNull { get; init; } = false;
    
    /// <summary>
    /// The priority of considering this Method over another.
    /// </summary>
    public int Priority { get; init; } = 0;
}