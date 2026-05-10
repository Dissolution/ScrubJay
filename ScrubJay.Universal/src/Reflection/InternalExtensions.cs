using System.Reflection;
using ScrubJay.Polyfills;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal.Reflection;

internal static class InternalExtensions
{
    extension(Type? type)
    {
        internal IReadOnlyList<MethodInfo> FindMatchingInstanceMethods(string? name, Type? returnType, params Type?[]? parameterTypes)
        {
            if (type is null)
                return [];

            Func<MethodInfo, bool> predicate = _ => true;

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
            
            // go through type and all subtypes to find a matching method
            var matching = new List<MethodInfo>();

            while (type is not null)
            {
                matching.AddMany(type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(predicate));
                if (type.IsByRefLike)
                    break;
                type = type.BaseType;
            }
            
            return matching;
        }
    }
}