using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection.Runtime;

[PublicAPI]
public static class RuntimeBuilder
{
    public static AssemblyName AssemblyName { get; } =  new("ScrubJay.Reflection.Runtime");

    public static AssemblyBuilder Assembly { get; } = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);

    public static ModuleBuilder Module { get; } = Assembly.DefineDynamicModule(AssemblyName.FullName);

    static RuntimeBuilder()
    {
     
    }
    
    public static DynamicMethod<D> CreateDynamicMethod<D>(string methodName)
        where D : Delegate
    {
        var invoke = Delegate.GetInvokeMethod<D>();
        var dynamicMethod = new DynamicMethod(
            name: methodName,
            attributes: MethodAttributes.Public | MethodAttributes.Static,
            callingConvention: CallingConventions.Standard,
            returnType: invoke.ReturnType,
            parameterTypes: invoke.GetParameterTypes(),
            m: Module,
            skipVisibility: true);

        return new DynamicMethod<D>(dynamicMethod);
    }
}