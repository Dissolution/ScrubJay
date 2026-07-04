namespace ScrubJay.Reflection.Lightweight;

public static class MethodExtensions
{
    extension(MethodBase? method)
    {
        [return: NotNullIfNotNull(nameof(method))]
        public Type[]? GetParameterTypes()
        {
            if (method is null) return null;
            var parameters = method.GetParameters();
            int count = parameters.Length;
            if (count == 0)
                return Type.EmptyTypes;
            Type[] parameterTypes = new Type[count];
            for (var i = 0; i < count; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType;
            }
            return parameterTypes;
        }
    }
}