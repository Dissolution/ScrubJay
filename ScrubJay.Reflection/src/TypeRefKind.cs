#pragma warning disable CA2217, MA0062, S4070, S2346, CA1008

using System.ComponentModel;

namespace ScrubJay.Reflection;

/// <summary>
/// The reference kind for a <see cref="Type"/>, <see cref="ParameterInfo"/>
/// </summary>
[PublicAPI]
[Flags]
public enum TypeRefKind
{
    /// <summary>
    /// Default referencing (copy value, ref class)
    /// </summary>
    [Description("")]
    Default = 0,

    /// <summary>
    /// <c>ref</c>
    /// </summary>
    [Description("ref ")]
    Ref = 1 << 0,

    /// <summary>
    /// <c>in</c>
    /// </summary>
    [Description("in ")]
    In = (1 << 1) | Ref,

    /// <summary>
    /// <c>out</c>
    /// </summary>
    [Description("out ")]
    Out = (1 << 2) | Ref,

    /// <summary>
    /// Any and all <see cref="TypeRefKind"/>s
    /// </summary>
    Any = Default | Ref | In | Out,
}