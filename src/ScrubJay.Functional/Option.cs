namespace ScrubJay.Functional;

[PublicAPI]
public static class Option
{
    [PublicAPI]
    [DebuggerDisplay("{ToString,nq}")]
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct None :
        IComparisonOperators<None, None, bool>,
        IEqualityOperators<None, None, bool>,
        ISpanParsable<None>,
        IParsable<None>,
        IComparable<None>,
        IEquatable<None>,
        ISpanFormattable,
        IFormattable
    {
        internal const int HASH_CODE = 0;

        public static bool operator ==(None left, None right) => true;
        public static bool operator !=(None left, None right) => false;
        public static bool operator >(None left, None right) => false;
        public static bool operator >=(None left, None right) => true;
        public static bool operator <(None left, None right) => false;
        public static bool operator <=(None left, None right) => true;

        static None IParsable<None>.Parse(string s, IFormatProvider? provider) => TryParse(s, provider).OkOrThrow();
        static bool IParsable<None>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out None result) => TryParse(s, provider).IsOk(out result);
        static None ISpanParsable<None>.Parse(text s, IFormatProvider? provider) => TryParse(s, provider).OkOrThrow();
        static bool ISpanParsable<None>.TryParse(text s, IFormatProvider? provider, out None result) => TryParse(s, provider).IsOk(out result);

        public static Result<None, Exception> TryParse([NotNullWhen(true)] string? str, IFormatProvider? _ = null)
        {
            if (str is null)
                return new ArgumentNullException(nameof(str));
            if (string.Equals(str, nameof(None), StringComparison.Ordinal))
                return default(None);
            return new FormatException($"Could not parse \"{str}\" into a {typeof(None)} value");
        }

        public static Result<None, Exception> TryParse(text txt, IFormatProvider? _ = null)
        {
            if (txt.Equals(nameof(None), StringComparison.Ordinal))
                return default(None);
            return new FormatException($"Could not parse \"{txt}\" into a {typeof(None)} value");
        }

        public int CompareTo(None _) => 0;

        public bool Equals(None _) => true;

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is null or None;

        public override int GetHashCode() => 0;

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, text format, IFormatProvider? provider) => TryFormat(destination, format, provider).IsOk(out charsWritten);

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