

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
    Default = 1 << 0,
    
    /// <summary>
    /// <c>ref</c>
    /// </summary>
    [Description("ref ")]
    Ref = 1 << 1,
    
    /// <summary>
    /// <c>in</c>
    /// </summary>
    [Description("in ")]
    In = (1 << 2) | Ref,
    
    /// <summary>
    /// <c>out</c>
    /// </summary>
    [Description("out ")]
    Out = (1 << 3) | Ref,

    Any = Default | Ref | In | Out,
}