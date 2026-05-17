namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        public bool IsDelegate => typeof(MulticastDelegate).IsAssignableFrom(type);

        public bool IsStatic => type is { IsAbstract: true, IsSealed: true };
        
        public Type[] GetGenericTypes()
        {
            if (type is null)
                return Type.EmptyTypes;
            return type.GetGenericArguments();
        }
    }
}