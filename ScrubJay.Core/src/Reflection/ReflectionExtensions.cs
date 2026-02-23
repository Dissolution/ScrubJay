using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Reflection;

[PublicAPI]
public static class ReflectionExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod New(string methodName,
            Type? returnType,
            params Type[]? parameterTypes)
        {
            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: typeof(ReflectionExtensions).Module,
                skipVisibility: true);

            return dynamicMethod;
        }

        public static DynamicMethod New<D>(string methodName)
            where D : Delegate
        {
            var invokeMethod = typeof(D).GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance).ThrowIfNull();
            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: invokeMethod.ReturnType,
                parameterTypes: invokeMethod.GetParameters().SelectToArray(static p => p.ParameterType),
                m: typeof(ReflectionExtensions).Module,
                skipVisibility: true);

            return dynamicMethod;
        }

        public static Result<D> TryCreate<D>(string methodName, Action<ILGenerator> generate)
            where D : Delegate
        {
            try
            {
                var dynamicMethod = DynamicMethod.New<D>(methodName);
                generate(dynamicMethod.GetILGenerator());
                return dynamicMethod.CreateDelegate<D>();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    extension(DynamicMethod dynamicMethod)
    {
        public bool TryCreateDelegate<D>([NotNullWhen(true)] out D? del)
            where D : Delegate
        {
            try
            {
                del = dynamicMethod.CreateDelegate<D>();
                return true;
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                del = null;
                return false;
            }
        }
    }
}