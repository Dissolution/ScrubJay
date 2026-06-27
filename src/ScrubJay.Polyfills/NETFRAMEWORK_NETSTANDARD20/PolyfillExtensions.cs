#if NETFRAMEWORK || NETSTANDARD2_0
// ReSharper disable CheckNamespace
#pragma warning disable IDE0160, IDE0161

namespace ScrubJay.Polyfills;

public static partial class PolyfillExtensions
{
    extension(Type? type)
    {
#pragma warning disable CA1822 // Mark members as static
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
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, "Invalid StringComparison"),
            };
        }
    }
}


#endif