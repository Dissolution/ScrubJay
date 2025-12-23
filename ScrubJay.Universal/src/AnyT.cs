using System.Reflection;
using System.Reflection.Emit;
using BF = System.Reflection.BindingFlags;

#if NET9_0_OR_GREATER
namespace ScrubJay.Universal;

[PublicAPI]
internal static partial class Any<T>
    where T : allows ref struct
{
    private static DynamicMethod CreateDynamicMethod(
        string name,
        Type returnType,
        params Type[] parameterTypes)
    {
        var dyn = new DynamicMethod(
            name: $"{typeof(T).FullName}_{name}",
            attributes: MethodAttributes.Public | MethodAttributes.Static,
            callingConvention: CallingConventions.Standard,
            returnType: returnType,
            parameterTypes: parameterTypes,
            m: typeof(Any).Module,
            skipVisibility: true);
        return dyn;
    }
    
    private static MethodInfo? FindMethod(
        Type instanceType, 
        BindingFlags flags,
        string name,
        Type returnType,
        params Type[] parameterTypes)
    {
        var methods = instanceType.GetMethods(flags);
        foreach (var method in methods)
        {
            if (method.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                method.ReturnType == returnType)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != parameterTypes.Length)
                    continue;

                bool match = true;
                for (var p = 0; p < parameters.Length; p++)
                {
                    if (parameters[p].ParameterType != parameterTypes[p])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return method;
            }
        }

        return null;
    }

    private static MethodInfo? FindMethod(Type instanceType, string name,
        Type returnType,
        params Type[] parameterTypes)
    {
        BindingFlags flags;
        
        // Enums
        if (instanceType.IsEnum)
        {
            // Enum instances use the methods on Enum
            instanceType = typeof(Enum);
        }

        // look for an instance method
        flags = BF.Public | BF.NonPublic | BF.Instance;
        
        // enum, ref-like, and value types we constrain
        if (instanceType.IsEnum || instanceType.IsByRefLike || instanceType.IsByRefLike || instanceType.IsValueType)
            flags |= BF.DeclaredOnly;
            
        return FindMethod(instanceType, flags, name, returnType, parameterTypes);
    }
    
    private static void EmitLoadInstance(ILGenerator gen, Type instanceType)
    {
        // stack types
        if (instanceType.IsEnum ||
            instanceType.IsByRef ||
            instanceType.IsByRefLike ||
            instanceType.IsValueType)
        {
            // load a ref to this value
            gen.Emit(OpCodes.Ldarga_S, 0);
        }
        // heap types
        else if (instanceType.IsClass || instanceType.IsInterface)
        {
            // load this class
            gen.Emit(OpCodes.Ldarg_0);
        }
        else
        {
            // we shouldn't be able to get here
            throw new InvalidOperationException($"Invalid type: `{instanceType}`");
        }
    }
    
    private static void EmitCallMethod(ILGenerator gen, Type instanceType, MethodInfo method)
    {
        // enums + byref likes we can use Constrained
        if (instanceType.IsByRef ||
            instanceType.IsEnum ||
            instanceType.IsByRefLike)
        {
            gen.Emit(OpCodes.Constrained, instanceType);
            gen.Emit(OpCodes.Callvirt, method);
        }
        // value types we can just call
        else if (instanceType.IsValueType)
        {
            gen.Emit(OpCodes.Call, method);
        }
        // class types we callvirt to account for overloads
        else
        {
            gen.Emit(OpCodes.Callvirt, method);
        }
    }
}
#endif