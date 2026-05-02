namespace ScrubJay.Text.Rendering;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public sealed class RenderToMethodAttribute : Attribute
{
    public bool AcceptsNull { get; init; } = false;
}