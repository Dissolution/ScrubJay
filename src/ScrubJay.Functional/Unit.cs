namespace ScrubJay.Functional;

[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct Unit :
#if NET7_0_OR_GREATER
    IEqualityOperators<Unit, Unit, bool>,
    IComparisonOperators<Unit, Unit, bool>,
#endif
    IEquatable<Unit>,
    IComparable<Unit>,
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IFormattable
{
    public static implicit operator Unit(ValueTuple _) => default;

    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
    public static bool operator <=(Unit left, Unit right) => true;
    public static bool operator >=(Unit left, Unit right) => true;
    public static bool operator <(Unit left, Unit right) => false;
    public static bool operator >(Unit left, Unit right) => false;

    public int CompareTo(Unit other) => 0;

    public bool Equals(Unit other) => true;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unit;

    public override int GetHashCode() => 1;
    
    public bool TryFormat(Span<char> destination, out int charsWritten, text _ = default, IFormatProvider? __ = default)
    {
        if (destination.Length >= 4)
        {
            destination[0] = 'U';
            destination[1] = 'n';
            destination[2] = 'i';
            destination[3] = 't';
            charsWritten = 4;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? _, IFormatProvider? __ = default)
    {
        return nameof(Unit);
    }

    public override string ToString() => nameof(Unit);

}