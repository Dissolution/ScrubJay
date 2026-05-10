using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal.Reflection;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod New(
            string methodName,
            Type? returnType,
            params Type[]? parameterTypes)
        {
            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: typeof(DynamicMethodExtensions).Module,
                skipVisibility: true);

            return dynamicMethod;
        }
        
        public static DynamicMethod New<D>(string methodName)
            where D : Delegate
        {
            var invokeMethod = typeof(D)
                .GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (invokeMethod is null)
                throw new InvalidOperationException("Could not find Delegate Invoke Method");
            var returnType = invokeMethod.ReturnType;
            var parameterTypes = Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType);
            return DynamicMethod.New(methodName, returnType, parameterTypes);
        }

        public static D? TryEmitDelegate<D>(
            string methodName,
            params ReadOnlySpan<(OpCode OpCode, object? Arg)> emissions)
            where D : Delegate
        {
            try
            {
                var dym = DynamicMethod.New<D>(methodName);
                var gen = dym.GetILGenerator();
                foreach (var emission in emissions)
                {
                    gen.Emit(emission);
                }
                return dym.CreateDelegate<D>();
            }
            catch // (Exception ex)
            {
                return null;
            }
        }
    }
    
    extension(DynamicMethod dynamicMethod)
    {
#if !NET6_0_OR_GREATER
        public D CreateDelegate<D>()
            where D : Delegate
            => (D)dynamicMethod.CreateDelegate(typeof(D));
#endif

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