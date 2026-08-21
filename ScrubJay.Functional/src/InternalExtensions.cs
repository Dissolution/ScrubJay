namespace ScrubJay.Functional;

internal static class InternalExtensions
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
    }
}