using System.Reflection.Emit;

namespace ScrubJay.Reflection;

[PublicAPI]
public static class RuntimeBuilder
{
    public static AssemblyBuilder Assembly { get; } 
    public static ModuleBuilder Module { get; }
}