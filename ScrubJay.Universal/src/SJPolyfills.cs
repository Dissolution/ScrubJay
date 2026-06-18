// ReSharper disable PartialTypeWithSinglePart
#pragma warning disable IDE0130, IDE0161


#if NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
    /// <summary>Defines a general-purpose Tuple implementation that allows access to Tuple instance members without knowing the underlying Tuple type.</summary>
    public interface ITuple
    {
        /// <summary>Returns the value of the specified <see langword="Tuple" /> element.</summary>
        /// <param name="index">The index of the specified <see langword="Tuple" /> element. <paramref name="index" /> can range from 0 for <see langword="Item1" /> of the <see langword="Tuple" /> to one less than the number of elements in the <see langword="Tuple" />.</param>
        /// <returns>The value of the specified <see langword="Tuple" /> element.</returns>
        object? this[int index] { get; }

        /// <summary>Gets the number of elements in this <see langword="Tuple" /> instance.</summary>
        /// <returns>The number of elements in this <see langword="Tuple" /> instance.</returns>
        int Length { get; }
    }
}

#endif

#if NETFRAMEWORK || NETSTANDARD2_0

namespace ScrubJay.Universal
{
    [PublicAPI]
    public static partial class PolyfillExtensions
    {
        extension(Type? type)
        {
#pragma warning disable CA1822
            public bool IsByRefLike => false;
#pragma warning restore CA1822
        }

        extension<T>(T[]? array)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Span<T> AsSpan(Range range)
            {
                if (array is null)
                    return [];
                (int start, int length) = range.GetOffsetAndLength(array.Length);
                return new Span<T>(array, start, length);
            }
        }

        extension(StringComparer)
        {
            public static StringComparer FromComparison(StringComparison comparisonType)
            {
                return comparisonType switch
                {
                    StringComparison.CurrentCulture => StringComparer.CurrentCulture,
                    StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
                    StringComparison.InvariantCulture => StringComparer.InvariantCulture,
                    StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
                    StringComparison.Ordinal => StringComparer.Ordinal,
                    StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
                    _ => throw new ArgumentOutOfRangeException(nameof(comparisonType)),
                };
            }
        }
    }
}
#endif

#if NETFRAMEWORK || NETSTANDARD
namespace System.Numerics
{
    [PublicAPI]
    public static class BitOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int offset)
            => (value << offset) | (value >> (32 - offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, int offset)
            => (value >> offset) | (value << (32 - offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RoundUpToPowerOf2(uint value)
        {
            // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
            --value;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }
    }
}

#endif

#if !NET7_0_OR_GREATER
namespace ScrubJay.Universal
{
    public static partial class PolyfillExtensions
    {
        extension(int)
        {
            public static bool IsEvenInteger(int value) => (value & 1) == 0;
        }
    }
}
#endif