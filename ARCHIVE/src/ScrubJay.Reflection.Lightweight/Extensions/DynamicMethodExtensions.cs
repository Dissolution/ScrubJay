namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod<D> CreateDynamicMethod<D>(
            string methodName,
            Module? module = null,
            bool skipVisibility = true)
            where D : Delegate
        {
            var invokeMethod = Delegate.GetInvokeMethod<D>();

            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: invokeMethod.ReturnType,
                parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
                m: module ?? typeof(DynamicMethodExtensions).Module,
                skipVisibility: skipVisibility);

            return new(dynamicMethod);
        }
        
        public static bool TryCreateDynamicMethod<D>(
            string methodName,
            [NotNullWhen(true)]
            out DynamicMethod<D>? dynamicMethod,
            Module? module = null,
            bool skipVisibility = true)
            where D : Delegate
        {
            var invokeMethod = Delegate.GetInvokeMethod<D>();

            try
            {
                var dm = new DynamicMethod(
                    name: methodName,
                    attributes: MethodAttributes.Public | MethodAttributes.Static,
                    callingConvention: CallingConventions.Standard,
                    returnType: invokeMethod.ReturnType,
                    parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
                    m: module ?? typeof(DynamicMethodExtensions).Module,
                    skipVisibility: skipVisibility);
                dynamicMethod = new(dm);
                return true;
            }
            catch (Exception)
            {
                dynamicMethod = null;
                return false;
            }
        }

        public static D GenerateDelegate<D>(
            string methodName,
            Action<ILGenerator> generateMethodBody,
            Module? module = null,
            bool skipVisibility = true)
            where D : Delegate
        {
            var dm = DynamicMethod.CreateDynamicMethod<D>(methodName, module, skipVisibility);
            generateMethodBody(dm.GetILGenerator());
            return dm.CreateDelegate();
        }
        
        public static bool TryGenerateDelegate<D>(
            string methodName,
            Action<ILGenerator> generateMethodBody,
            [NotNullWhen(true)] out D? generatedDelegate,
            Module? module = null,
            bool skipVisibility = true)
            where D : Delegate
        {
            if (!DynamicMethod.TryCreateDynamicMethod<D>(methodName, out var dm, module, skipVisibility))
            {
                generatedDelegate = null;
                return false;
            }
            generateMethodBody(dm.GetILGenerator());
            return dm.TryCreateDelegate(out generatedDelegate);
        }
    }
}