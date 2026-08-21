namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        [NotNullIfNotNull(nameof(type))]
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(type))]
            get
            {
                if (type is not null)
                {
                    Type parentType = type.DeclaringType ?? type.ReflectedType ?? type.Module.GetType();
                    Debug.Assert(parentType is not null);
                    return parentType;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="TypeAlias"/> for the given <see cref="Type"/>.
        /// </summary>
        public string Alias
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => TypeAlias.For(type);
        }
    }
}