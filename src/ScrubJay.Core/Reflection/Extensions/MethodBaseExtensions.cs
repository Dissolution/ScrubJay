using System.Reflection;

namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class MethodBaseExtensions
{
    extension(MethodBase method)
    {
        public Type[] GetParameterTypes()
        {
            return Array.ConvertAll(method.GetParameters(), static parameter => parameter.ParameterType);
        }
    }
}