using System.Globalization;

namespace ScrubJay.Text.Benchmarks.BDN;

public static class Comparers
{
    public static StringComparer NumericStringComparer { get; }
#if NET7_0_OR_GREATER
        = StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);
#else
        = StringComparer.Ordinal;
#endif

    internal static StringLengthOrdinalComparer StringLengthOrdinalComparer { get; } = new();

}