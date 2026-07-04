namespace ScrubJay.Reflection.Lightweight;

public static class TypeExtensions
{
    extension(Type? type)
    {
        internal IEnumerable<Type> InvokableTypes()
        {
            while (type is not null)
            {
                yield return type;
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
                if (type.IsByRefLike)
                    yield break;
#endif
                type = type.BaseType;
            }
        }

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