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
        [NotNullIfNotNull(nameof(type))]
        public Type? ParentType
        {
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
                return string.Equals(genericDef.Namespace, "System", StringComparison.Ordinal) &&
                       (genericDef.Name.StartsWith("Tuple`", StringComparison.Ordinal) ||
                        genericDef.Name.StartsWith("ValueTuple`", StringComparison.Ordinal));
            }
        }
    }
}