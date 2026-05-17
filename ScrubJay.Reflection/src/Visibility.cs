using System.ComponentModel;

namespace ScrubJay.Reflection;

[PublicAPI]
[Flags]
public enum Visibility
{
    None = 0,
    [Description("instance")]
    Instance = 1 << 0,
    [Description("static")]
    Static = 1 << 1,
    [Description("public")]
    Public = 1 << 2,
    [Description("internal")]
    Internal = 1 << 3,
    [Description("protected")]
    Protected = 1 << 4,
    [Description("private")]
    Private = 1 << 5,
    
    NonPublic = Internal | Protected | Private,
}