namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        public IEnumerable<MethodInfo> FindMatchingMethods<D>(string? name)
            where D : Delegate
        {
            var invoke = Delegate.GetInvokeMethod<D>();
            return type.FindMatchingMethods(name, invoke.ReturnType, invoke.GetParameterTypes());
        }

        public IEnumerable<MethodInfo> FindMatchingMethods(
            string? name,
            Type? returnType = null,
            Type?[]? parameterTypes = null)
        {
            var flags = BindingFlags.AllVisibilities | BindingFlags.DeclaredOnly;

            while (type is not null)
            {
                var methods = type.GetMethods(flags);
                foreach (var method in methods)
                {
                    if (!matchName(method)) continue;
                    if (!matchDelegate(method)) continue;
                    yield return method;
                }
                type = type.BaseType;
            }

            yield break;

            bool matchName(MethodInfo method)
            {
                if (name is null) return true;
                return string.Equals(method.Name, name, StringComparison.Ordinal);
            }

            bool matchDelegate(MethodInfo method)
            {
                if (returnType is not null)
                {
                    // the method's return type must be assignable to the delegate's return type
                    if (!method.ReturnType.IsAssignableTo(returnType))
                        return false;
                }

                if (parameterTypes is not null)
                {

                    var methodParameterTypes = method.GetParameterTypes();

                    // todo: default parameters

                    if (method.IsStatic)
                    {
                        // static methods must match all parameters 1:1
                        if (methodParameterTypes.Length != parameterTypes.Length)
                            return false;
                        for (var i = 0; i < parameterTypes.Length; i++)
                        {
                            // delegate parameter must be assignable to method parameter
                            var parameterType = parameterTypes[i];
                            if (parameterType is not null && !parameterType.IsAssignableTo(methodParameterTypes[i]))
                                return false;
                        }
                        // okay
                        return true;
                    }

                    var instanceType = method.ParentType;

                    // instance methods require the first delegate parameter to be the instance type
                    if (parameterTypes.Length == 0)
                        return false;

                    var firstParam = parameterTypes[0];
                    if (firstParam is not null && !firstParam.IsAssignableTo(instanceType))
                        return false;

                    // then the rest have to match
                    if (methodParameterTypes.Length != parameterTypes.Length - 1)
                        return false;
                    for (var i = 0; i < methodParameterTypes.Length; i++)
                    {
                        // delegate parameter must be assignable to method parameter
                        var parameterType = parameterTypes[i + 1];
                        if (parameterType is not null && !parameterType.IsAssignableTo(methodParameterTypes[i]))
                            return false;
                    }
                    // okay
                    return true;

                }

                return true;
            }
        }
    }
}