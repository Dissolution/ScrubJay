namespace ScrubJay.Universal;

/// <summary>
/// Extensions on <see cref="Type"/>
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        /// <summary>
        /// Gets the parent (owning) <see cref="Type"/> of this <see cref="Type"/>.
        /// </summary>
        //[return: NotNullIfNotNull(nameof(type))]
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(type))]
            get
            {
                if (type is null)
                    return null;
                return type.DeclaringType ??
                       type.ReflectedType ??
                       type.Module.GetType();
            }
        }

        /// <summary>
        /// Is this <see cref="Type"/> declared as <c>static</c>?
        /// </summary>
        public bool IsStatic => type is { IsAbstract: true, IsSealed: true };

        /// <summary>
        /// Is this <see cref="Type"/> any of <c>Tuple&lt;T0, ..., TN&gt;</c> or <c>ValueTuple&lt;T0, ..., TN&gt;</c>?
        /// </summary>
        public bool IsTuple
        {
            get
            {
                if (type is null || !type.IsGenericType)
                    return false;
                var genericDef = type.GetGenericTypeDefinition();
                return genericDef.Namespace == "System" &&
                       (genericDef.Name.StartsWith("Tuple`") ||
                        genericDef.Name.StartsWith("ValueTuple`"));
            }
        }
    }
}