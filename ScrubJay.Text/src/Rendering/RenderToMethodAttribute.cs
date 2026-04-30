using ScrubJay.Universal.Extensions;

namespace ScrubJay.Text.Rendering;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public sealed class RenderToMethodAttribute : Attribute;