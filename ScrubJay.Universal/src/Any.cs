using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

/// <summary>
/// A static utility class for working with <b>any</b> generic values,<br/>
/// including ones with the <c>allows ref struct</c> anti-constraint.
/// </summary>
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct"/>
[PublicAPI]
public static partial class Any
{
}

#if NET9_0_OR_GREATER
/// <summary>
/// Internal helper utility for .net9.0+ that dynamically creates and caches delegates that work on <c>ref struct</c>s.
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
internal static partial class MethodCache<T>
    where T : allows ref struct
{
    private static void EmitLoadInstance(ILGenerator generator, Type instanceType)
    {
        // stack types
        if (instanceType.IsEnum || instanceType.IsByRef || instanceType.IsByRefLike || instanceType.IsValueType)
        {
            // load a ref to this value
            generator.Emit(
                OpCodes.Ldarga_S,
                0);
        }
        // heap types
        else
        {
            // load the value directly
            generator.Emit(OpCodes.Ldarg_0);
        }
    }

    
    private static MethodInfo? FindBestMethod<D>(Type? type, string name)
        where D : Delegate
    {
        var invokeMethod = typeof(D).GetMethod("Invoke")!;
        Type returnType = invokeMethod.ReturnType;
        Type[] parameterTypes = Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType);

        Func<MethodInfo, bool> isMatchingMethod = m =>
        {
            if (m.Name != name || !m.ReturnType.IsAssignableTo(returnType))
                return false;
            var mp = m.GetParameters();
            if (mp.Length != parameterTypes.Length)
                return false;
            for (var i = 0; i < mp.Length; i++)
            {
                if (!mp[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                    return false;
            }
            return true;
        };
        
        // go through type and all subtypes to find a matching method
        
        while (type is not null)
        {
            MethodInfo? method = type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(isMatchingMethod)
                .FirstOrDefault();
            if (method is not null)
                return method;
            if (type.IsByRefLike)
                return null; // only methods declared directly on the ref struct can be used
            type = type.BaseType;
        }
        return null;
    }
    
}
#endif