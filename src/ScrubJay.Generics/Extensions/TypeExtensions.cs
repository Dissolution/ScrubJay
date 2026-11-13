namespace ScrubJay.Generics.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
        /// <summary>
        /// Is this <see cref="Type"/> declared as <see langword="static"/>?
        /// </summary>
        public bool IsStatic
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type is { IsAbstract: true, IsSealed: true };
        }

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

        [NotNullIfNotNull(nameof(type))]
        public string? Alias => TypeAlias.For(type);

#if NETSTANDARD2_0
        public bool IsByRefLike => false;
#endif
    }
}