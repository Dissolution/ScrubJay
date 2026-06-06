namespace ScrubJay.Reflection;

[PublicAPI]
public static class RuntimeBuilder
{
    public static AssemblyName AssemblyName { get; } =  new("ScrubJay.Reflection.Runtime");

    public static AssemblyBuilder Assembly { get; } = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);

    public static ModuleBuilder Module { get; } = Assembly.DefineDynamicModule(AssemblyName.FullName);

    static RuntimeBuilder()
    {
     
    }
}