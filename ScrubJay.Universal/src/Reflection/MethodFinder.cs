using System.Reflection;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal.Reflection;

internal static class MethodFinder
{
    private static Func<MethodInfo, bool> GetMatchPredicate(string? name, Type? returnType, Type?[]? parameterTypes)
    {
        Func<MethodInfo, bool>? predicate = null;

        if (!string.IsNullOrEmpty(name))
        {
            predicate &= (method => string.Equals(method.Name, name, StringComparison.Ordinal));
        }

        if (returnType is not null)
        {
            predicate &= (method => method.ReturnType.IsAssignableTo(returnType));
        }

        if (parameterTypes is not null)
        {
            int parameterCount = parameterTypes.Length;
            predicate &= (method =>
            {
                var mp = method.GetParameters();
                if (mp.Length != parameterCount)
                    return false;
                for (var i = 0; i < mp.Length; i++)
                {
                    var pt = parameterTypes[i];
                    if (pt is null) continue;
                    if (!mp[i].ParameterType.IsAssignableFrom(pt))
                        return false;
                }
                return true;
            });
        }

        return predicate ?? Predicate<MethodInfo>.True;
    }
    
    public static IEnumerable<MethodInfo> FindMatchingMethods(
        this Type? type,
        BindingFlags bindingFlags,
        string? name, 
        Type? returnType, 
        params Type?[]? parameterTypes)
    {
        var match = GetMatchPredicate(name, returnType, parameterTypes);
            
        // go through type and all subtypes to find a matching method
        bindingFlags |= BindingFlags.DeclaredOnly;

        while (type is not null)
        {
            foreach (var method in type
                .GetMethods(bindingFlags)
                .Where(match))
            {
                yield return method;
            }
            if (type.IsByRefLike)
                break;
            type = type.BaseType;
        }
    }

    public static IEnumerable<MethodInfo> FindMatchingInstanceMethods(
        this Type? type,
        string? name,
        Type? returnType,
        params Type?[]? parameterTypes)
        => FindMatchingMethods(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, name, returnType, parameterTypes);
    
    public static IEnumerable<MethodInfo> FindMatchingStaticMethods(
        this Type? type,
        string? name,
        Type? returnType,
        params Type?[]? parameterTypes)
        => FindMatchingMethods(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, name, returnType, parameterTypes);

}