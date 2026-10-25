namespace ScrubJay.Reflection.Lightweight.Extensions;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        public static DynamicMethod Create(
            string methodName,
            Type? returnType = null,
            Type[]? parameterTypes = null,
            Module? module = null,
            bool skipVisibility = true)
        {
            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: module ?? Runtime.Module,
                skipVisibility: skipVisibility);

            return dynamicMethod;
        }
        
        public static DynamicMethod<D> Create<D>(
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
                parameterTypes: invokeMethod.GetParameterTypes(),
                m: module ?? Runtime.Module,
                skipVisibility: skipVisibility);

            return new(dynamicMethod);
        }
    }
}