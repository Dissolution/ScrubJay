namespace ScrubJay.Reflection.Lightweight.Extensions;

public static class TypeExtensions
{
    extension(Type? type)
    {
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

        /// <summary>
        /// Can instances of this <see cref="Type"/> be <see langword="null"/>?
        /// </summary>
        /// <remarks>
        /// A <see langword="null"/> <see cref="Type"/> will return <see langword="true"/>.
        /// </remarks>
        public bool CanBeNull
            => type is null || !type.IsValueType || Nullable.GetUnderlyingType(type) != null;

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