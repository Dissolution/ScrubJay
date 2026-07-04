namespace ScrubJay.Reflection.Lightweight;

public static class EnumerableExtensions
{
    extension(IEnumerable<MethodInfo> methods)
    {
        public IEnumerable<MethodInfo> WithShape(
            string? name = null,
            Type? returnType = null,
            Type[]? parameterTypes = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                methods = methods.Where(method => string.Equals(method.Name, name, StringComparison.Ordinal));
            }

            if (returnType is not null)
            {
                methods = methods.Where(method => method.ReturnType == returnType);
            }

            if (parameterTypes is not null)
            {
                methods = methods.Where(method =>
                {
                    var methodParameterTypes = method.GetParameterTypes();
                    if (methodParameterTypes.Length != parameterTypes.Length)
                        return false;
                    for (var i = 0; i < parameterTypes.Length; i++)
                    {
                        if (methodParameterTypes[i] != parameterTypes[i])
                            return false;
                    }
                    return true;
                });
            }

            return methods;
        }
        
        public IEnumerable<MethodInfo> WithShape<D>()
            where D : Delegate
        {
            var invokeMethod = Delegate.GetInvokeMethod<D>();

            string name = typeof(D).Name;
            Type returnType = invokeMethod.ReturnType;
            Type[] parameterTypes = invokeMethod.GetParameterTypes();
            return methods.WithShape(name, returnType, parameterTypes);
        }
        
        public IEnumerable<MethodInfo> WithShape<D>(D? @delegate)
            where D : Delegate
        {
            var invokeMethod = Delegate.GetInvokeMethod<D>();

            string name = typeof(D).Name;
            Type returnType = invokeMethod.ReturnType;
            Type[] parameterTypes = invokeMethod.GetParameterTypes();
            return methods.WithShape(name, returnType, parameterTypes);
        }
    }
}