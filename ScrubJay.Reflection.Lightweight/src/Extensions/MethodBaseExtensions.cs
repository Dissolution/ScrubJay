namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class MethodBaseExtensions
{
    extension(MethodBase? methodBase)
    {
        [return: NotNullIfNotNull(nameof(methodBase))]
        public Type[]? GetParameterTypes()
        {
            if (methodBase is null)
                return null;
            var parameters = methodBase.GetParameters();
            return Array.ConvertAll<ParameterInfo, Type>(parameters, static param => param.ParameterType);
            

        }
        
        public Type? GetSystemDelegateType()
        {
            if (methodBase is null)
                return null;

            var parameterTypes = methodBase.GetParameterTypes();
            int parameterCount = parameterTypes.Length;
            Type? returnType;

            if (methodBase is MethodInfo method)
            {
                returnType = method.ReturnType;
            }
            else if (methodBase is ConstructorInfo ctor)
            {
                returnType = ctor.DeclaringType!;
            }
            else
            {
                throw new NotImplementedException();
            }
            
            InterpolatedBuilder nameBuilder = new();
            
            if (returnType == typeof(void))
            {
                nameBuilder.AppendLiteral("System.Action");
                if (parameterCount > 0)
                {
                    if (parameterCount > 16) // 1-16 okay for action
                        return null;
                    
                    nameBuilder.AppendLiteral("<");
                    Span<char> commas = new char[parameterCount - 1];
                    commas.Fill(',');
                    nameBuilder.AppendFormatted(commas);
                    nameBuilder.AppendLiteral(">");
                }
            }
            else
            {
                nameBuilder.AppendLiteral("System.Func<");
                if (parameterCount > 0)
                {
                    if (parameterCount > 17) // 1 - 17 okay for func
                        return null;
                    
                    Span<char> commas = new char[parameterCount];
                    commas.Fill(',');
                    nameBuilder.AppendFormatted(commas);
                }
                nameBuilder.AppendLiteral(">");
            }

            string name = nameBuilder.ToStringAndClear();
            Type? type = Type.GetType(name, false);
            if (type is null)
                return null;
            
            // if this is a func, we have to account for the return type
            if (returnType != typeof(void))
            {
                Array.Resize(ref parameterTypes,  parameterCount + 1);
                parameterTypes[^1] = returnType!;
            }

            Type delegateType = type.MakeGenericType(parameterTypes);
            return delegateType;
        }
    }
}