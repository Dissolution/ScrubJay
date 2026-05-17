namespace ScrubJay.Reflection;

[PublicAPI]
public static class RuntimeBuilder
{
    public static AssemblyBuilder Assembly { get; }
    
    public static ModuleBuilder Module { get; }

    static RuntimeBuilder()
    {
        Assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("ScrubJay.Reflection.Runtime"),
            AssemblyBuilderAccess.Run);
        Module = Assembly.DefineDynamicModule("ScrubJay.Reflection.Runtime");
    }
}