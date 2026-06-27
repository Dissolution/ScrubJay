namespace ScrubJay.Reflection.Lightweight;

public static class TypeExtensions
{
    extension(Type? type)
    {
        public IEnumerable<Type> EnumerateTypeAndBaseTypes()
        {
            while (type is not null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}