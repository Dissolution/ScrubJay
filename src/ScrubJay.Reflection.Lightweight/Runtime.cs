namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class Runtime
{
    public static AssemblyName AssemblyName { get; } = new("ScrubJay.Reflection.Runtime");

#if NETFRAMEWORK
    public static AssemblyBuilder Assembly { get; } = AppDomain.CurrentDomain.DefineDynamicAssembly(new("ScrubJay.Reflection.Lightweight.Runtime"), AssemblyBuilderAccess.Run);
#else
    public static AssemblyBuilder Assembly { get; } = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);
#endif
    public static ModuleBuilder Module { get; } = Assembly.DefineDynamicModule(AssemblyName.FullName);


    public static D? TryGenerateDelegate<D>(
        string? delegateName,
        Action<ILGenerator> generateBody)
        where D : Delegate
    {
        var invokeMethod = Delegate.GetInvokeMethod<D>();

        var name = delegateName ?? typeof(D).Name;
        var returnType = invokeMethod.ReturnType;
        var parameterTypes = invokeMethod.GetParameterTypes();

#if NETFRAMEWORK
        // Dynamic Method in NetFramework does not support byref-like return types
        if (returnType.IsByRef)
        {
            // Can use TypeBuilder
            try
            {
                TypeBuilder typeBuilder = Module.DefineType(
                    $"{name}_holder",
                    TypeAttributes.Public | TypeAttributes.Static | TypeAttributes.Class);
                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    name: name,
                    attributes: MethodAttributes.Public | MethodAttributes.Static,
                    callingConvention: CallingConventions.Standard,
                    returnType: returnType,
                    parameterTypes: parameterTypes);
                var ilGenerator = methodBuilder.GetILGenerator();
                generateBody(ilGenerator);

                var type = typeBuilder.CreateType();
                var method = type.GetMethod(methodBuilder.Name, BindingFlags.Public | BindingFlags.Static);
                D? del = method.CreateDelegate<D>();
                return del;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return null;
            }
        }
        else
        {
#endif
        // Can use DynamicMethod
        try
        {
            var dynamicMethod = new DynamicMethod(
                name: name,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: Module,
                skipVisibility: true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            generateBody(ilGenerator);

            D? del = dynamicMethod.CreateDelegate<D>();
            return del;
        }
        catch (Exception ex)
        {
            Debugger.Break();
            return null;
        }
#if NETFRAMEWORK
        }
#endif
    }
}