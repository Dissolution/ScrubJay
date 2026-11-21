using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod Create<D>(string methodName)
            where D : Delegate
        {
            var invokeMethod = typeof(D).GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;

            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: invokeMethod.ReturnType,
                parameterTypes: invokeMethod.GetParameterTypes(),
                m: RuntimeBuilder.Module,
                skipVisibility: true);

            return dynamicMethod;
        }
    }
}