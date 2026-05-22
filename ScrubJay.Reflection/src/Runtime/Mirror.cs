using ScrubJay.Universal;

namespace ScrubJay.Reflection.Runtime;

public static partial class Mirror
{
    public static AssemblyName AssemblyName { get; } = new AssemblyName("ScrubJay.Mirror");
    
    public static AssemblyBuilder AssemblyBuilder { get; } = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);

    public static ModuleBuilder ModuleBuilder { get; } = AssemblyBuilder.DefineDynamicModule("ScrubJay.Mirror");
    
    public static DynamicMethod<D> CreateDynamicMethod<D>(string methodName)
        where D : Delegate
    {
        var invokeMethod = typeof(D)
            .GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        
        if (invokeMethod is null)
            throw new InvalidOperationException("Could not find Delegate Invoke Method");

        var dynamicMethod = new DynamicMethod(
            name: methodName,
            attributes: MethodAttributes.Public | MethodAttributes.Static,
            callingConvention: CallingConventions.Standard,
            returnType: invokeMethod.ReturnType,
            parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
            m: typeof(Any).Module,
            skipVisibility: true);

        return new DynamicMethod<D>(dynamicMethod);
    }

    public static dynamic Search<T>() => throw new NotImplementedException();
}