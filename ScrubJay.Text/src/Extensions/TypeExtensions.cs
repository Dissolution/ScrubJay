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
            if (interfaceType is null || !interfaceType.IsInterface)
                return false;

            Type? baseType = type;
            while (baseType is not null)
            {
                Type[] interfaces = baseType.GetInterfaces();
                Debug.Assert(interfaces is not null);
                Debug.Assert(interfaces.All(i => i is not null));

                foreach (var it in interfaces!)
                {
                    if (it == interfaceType || it.ImplementsInterface(interfaceType))
                        return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
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