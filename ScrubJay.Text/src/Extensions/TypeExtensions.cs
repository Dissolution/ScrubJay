namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        public bool IsTuple
        {
            get
            {
                if (type is not null)
                {
                    if (type.Namespace == "System" && (text.StartsWith(type.Name, "Tuple") || text.StartsWith(type.Name, "ValueTuple")))
                        return true;
                }
                return false;
            }
        }

        public bool ImplementsInterface(Type? interfaceType)
        {
            return type is not null &&
                interfaceType is not null &&
                interfaceType.IsInterface &&
                type.GetInterfaces().Contains(interfaceType);
        }
        
        public bool ImplementsInterface<I>()
            where I : class
        {
            return type is not null &&
                typeof(I).IsInterface &&
                type.GetInterfaces().Contains(typeof(I));
        }

        public IEnumerable<Type> BaseTypes()
        {
            Type? baseType = type?.BaseType;
            while (baseType is not null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }
    }
}