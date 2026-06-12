namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        public bool IsDelegate => typeof(MulticastDelegate).IsAssignableFrom(type);

        public bool IsStatic => type is { IsAbstract: true, IsSealed: true };

        public bool CanBeNull
        {
            get
            {
                if (type is null)
                    return true;
                if (type.IsByRefLike)
                    return false;
                if (type.IsValueType)
                    return Nullable.GetUnderlyingType(type) is not null;
                return true;
            }
        }
        
        public Type[] GetGenericTypes()
        {
            if (type is null)
                return Type.EmptyTypes;
            return type.GetGenericArguments();
        }

        public Type[] GetBaseTypes()
        {
            if (type is null) return [];
            
            List<Type> baseTypes = new(8);
            Type? baseType = type.BaseType;
            while (baseType is not null)
            {
                baseTypes.Add(baseType);
                baseType = baseType.BaseType;
            }
            return baseTypes.ToArray();
        }

        public Option<Type> GenericTypeDefinition
        {
            get
            {
                if (type is null || !type.IsGenericType)
                    return None;
                return type.GetGenericTypeDefinition();
            }
        }
        
        public bool Implements<T>() => type.Implements(typeof(T));

        public bool Implements(Type? other)
        {
            if (ReferenceEquals(type, other))
                return true;
            if (type is null || other is null)
                return false;

            // todo
            return false;
        }
    }
}