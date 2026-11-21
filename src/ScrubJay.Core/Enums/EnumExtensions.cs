// We're using these parameters

#pragma warning disable IDE0060

// ReSharper disable EntityNameCapturedOnly.Global

using static InlineIL.IL;

namespace ScrubJay.Enums;

/// <summary>
/// Generic (non-boxing) extensions upon <c>enum</c>
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
        public static bool operator ==(E left, E right) => left.EqualTo(right);
        public static bool operator !=(E left, E right) => !left.EqualTo(right);

        public static bool operator <(E left, E right) => left.CompareTo(right) < 0;
        public static bool operator <=(E left, E right) => left.CompareTo(right) <= 0;
        public static bool operator >(E left, E right) => left.CompareTo(right) > 0;
        public static bool operator >=(E left, E right) => left.CompareTo(right) >= 0;

        public static Type UnderlyingType => typeof(E).GetEnumUnderlyingType();
        public static int Size => Unsafe.SizeOf<E>();
    }

    extension<E>(E @enum)
        where E : struct, Enum
    {
        /// <summary>
        /// Is this <see langword="enum"/> the <see langword="default"/> value for its <see cref="Type"/>?
        /// </summary>
        public bool IsDefault
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualTo(E other) => EqualityComparer<E>.Default.Equals(@enum, other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(E other) => Comparer<E>.Default.Compare(@enum, other);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(E other) => Comparer<E>.Default.Compare(@enum, other) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThanOrEqual(E other) => Comparer<E>.Default.Compare(@enum, other) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(E other) => Comparer<E>.Default.Compare(@enum, other) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThanOrEqual(E other) => Comparer<E>.Default.Compare(@enum, other) >= 0;
    }
}