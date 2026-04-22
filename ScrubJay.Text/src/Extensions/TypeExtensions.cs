namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(type))]
            get
            {
                if (type is not null)
                {
                    if (type.DeclaringType is not null)
                        return type.DeclaringType;
                    if (type.ReflectedType is not null)
                        return type.ReflectedType;
                    return type.Module.GetType();
                }
                return null;
            }
        }
        
        public bool IsTuple
        {
            get
            {
                if (type is not null)
                {
                    if (type.Namespace == "System" && (type.Name.StartsWith("Tuple") || type.Name.StartsWith("ValueTuple")))
                        return true;
                }
                return false;
            }
        }
    }
}