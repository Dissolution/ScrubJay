namespace ScrubJay.Reflection.Lightweight;

public static class TypeExtensions
{
    extension(Type? type)
    {
        public bool IsTuple
        {
            get
            {
#if !NETSTANDARD2_0
                return typeof(ITuple).IsAssignableFrom(type);
#else
                if (type is null || !type.IsGenericType)
                    return false;

                var gtd = type.GetGenericTypeDefinition();
                return
                    gtd == typeof(ValueTuple<>) ||
                    gtd == typeof(ValueTuple<,>) ||
                    gtd == typeof(ValueTuple<,,>) ||
                    gtd == typeof(ValueTuple<,,,>) ||
                    gtd == typeof(ValueTuple<,,,,>) ||
                    gtd == typeof(ValueTuple<,,,,,>) ||
                    gtd == typeof(ValueTuple<,,,,,,>) ||
                    gtd == typeof(ValueTuple<,,,,,,,>) ||
                    gtd == typeof(Tuple<>) ||
                    gtd == typeof(Tuple<,>) ||
                    gtd == typeof(Tuple<,,>) ||
                    gtd == typeof(Tuple<,,,>) ||
                    gtd == typeof(Tuple<,,,,>) ||
                    gtd == typeof(Tuple<,,,,,>) ||
                    gtd == typeof(Tuple<,,,,,,>) ||
                    gtd == typeof(Tuple<,,,,,,,>);
#endif
            }
        }

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