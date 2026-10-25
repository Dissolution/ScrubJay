namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class Runtime
{
    public static AssemblyName AssemblyName { get; } = new("ScrubJay.Reflection.Runtime");

#if NETFRAMEWORK
    public static AssemblyBuilder Assembly { get; } = AppDomain.CurrentDomain.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);
#else
    public static AssemblyBuilder Assembly { get; } = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);
#endif
    
    public static ModuleBuilder Module { get; } = Assembly.DefineDynamicModule(AssemblyName.FullName);
}