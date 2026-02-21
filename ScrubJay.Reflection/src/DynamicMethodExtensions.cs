namespace ScrubJay.Reflection;

[PublicAPI]
public static class DynamicMethodExtensions
{
    extension(DynamicMethod)
    {
        /// <summary>
        /// Creates a new <see cref="DynamicMethod"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public static DynamicMethod New(string name, Type? returnType, Type[]? parameterTypes = null)
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