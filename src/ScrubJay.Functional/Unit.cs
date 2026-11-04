#pragma warning disable IDE0060 // Remove unused parameter

namespace ScrubJay.Functional;

[PublicAPI]
[DebuggerDisplay("{ToString,nq}")]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct Unit :
    IComparisonOperators<Unit, Unit, bool>,
    IEqualityOperators<Unit, Unit, bool>,
    ITrySpanParsable<Unit>,
    IComparable<Unit>,
    IEquatable<Unit>,
    ITrySpanFormattable
{
    public static readonly Unit Default;

    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
    public static bool operator >(Unit left, Unit right) => false;
    public static bool operator >=(Unit left, Unit right) => true;
    public static bool operator <(Unit left, Unit right) => false;
    public static bool operator <=(Unit left, Unit right) => true;

    public static Result<Unit, Exception> TryParse([NotNullWhen(true)] string? str, IFormatProvider? _ = null)
    {
        if (str is null)
            return new ArgumentNullException(nameof(str));
        if (string.Equals(str, nameof(Unit), StringComparison.Ordinal))
            return default(Unit);
        return new FormatException($"Could not parse \"{str}\" into a {nameof(Unit)} value");
    }

    public static Result<Unit, Exception> TryParse(text txt, IFormatProvider? _ = null)
    {
        if (txt.Equals(nameof(Unit), StringComparison.Ordinal))
            return default(Unit);
        return new FormatException($"Could not parse \"{txt}\" into a {nameof(Unit)} value");
    }

    public int CompareTo(Unit _) => 0;

    public bool Equals(Unit _) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override int GetHashCode() => 1;

    public Result<int, Exception> TryFormat(Span<char> destination, text _ = default, IFormatProvider? __ = default)
    {
        if (destination.Length < nameof(Unit).Length)
            return new ArgumentOutOfRangeException(nameof(destination), destination.Length, $"Destination length must be at least {nameof(Unit).Length}");
        nameof(Unit).CopyTo(destination);
        return nameof(Unit).Length;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => nameof(Unit);

    public override string ToString() => nameof(Unit);
}