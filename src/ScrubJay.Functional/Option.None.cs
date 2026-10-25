#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable IDE0060 // Remove unused parameter

namespace ScrubJay.Functional;

public static partial class Option
{
    /// <summary>
    /// <b>Option.None</b> indicates the absence of a <c>T</c> value in an <see cref="Option{T}"/>.
    /// </summary>
    /// <remarks>
    /// - <c>None</c> is <see langword="false"/>-y.
    /// </remarks>
    [PublicAPI]
    [DebuggerDisplay("{ToString,nq}")]
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct None :
        IComparisonOperators<None, None, bool>,
        IEqualityOperators<None, None, bool>,
        IBitwiseOperators<None, None, bool>,
        ITrySpanParsable<None>,
        IComparable<None>,
        IEquatable<None>,
        ITrySpanFormattable
    {
        public static implicit operator bool(None none) => false;

        public static bool operator true(None none) => false;
        public static bool operator false(None none) => true;

        public static bool operator !(None none) => true;
        public static bool operator |(None left, None right) => false;
        public static bool operator &(None left, None right) => false;
        public static bool operator ^(None left, None right) => false;

        public static bool operator ==(None left, None right) => true;
        public static bool operator !=(None left, None right) => false;
        public static bool operator >(None left, None right) => false;
        public static bool operator >=(None left, None right) => true;
        public static bool operator <(None left, None right) => false;
        public static bool operator <=(None left, None right) => true;


        public static Result<None, Exception> TryParse([NotNullWhen(true)] string? str, IFormatProvider? _ = null)
        {
            if (str is null)
                return new ArgumentNullException(nameof(str));
            if (string.Equals(str, nameof(None), StringComparison.Ordinal))
                return default(None);
            return new FormatException($"Could not parse \"{str}\" into a {nameof(None)} value");
        }

        public static Result<None, Exception> TryParse(text txt, IFormatProvider? _ = null)
        {
            if (txt.Equals(nameof(None), StringComparison.Ordinal))
                return default(None);
            return new FormatException($"Could not parse \"{txt}\" into a {nameof(None)} value");
        }

        public int CompareTo(None _) => 0;

        public bool Equals(None _) => true;

        public override bool Equals(object? obj) => obj is null or None;

        public override int GetHashCode() => 0;

        public Result<int, Exception> TryFormat(Span<char> destination, text _ = default, IFormatProvider? __ = default)
        {
            if (destination.Length < nameof(None).Length)
                return new ArgumentOutOfRangeException(nameof(destination), destination.Length, $"Destination length must be at least {nameof(None).Length}");
            nameof(None).CopyTo(destination);
            return nameof(None).Length;
        }

        public string ToString(string? format, IFormatProvider? formatProvider) => nameof(None);

        public override string ToString() => nameof(None);
    }
}