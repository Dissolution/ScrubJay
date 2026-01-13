using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Reflection;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod New(string name, Type? returnType, params Type[]? parameterTypes)
        {
            var dyn = new DynamicMethod(
                name: name,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: RuntimeBuilder.Module,
                skipVisibility: true);
            return dyn;
        }
    }
}