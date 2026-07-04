namespace ScrubJay.Functional.Implementations;

[PublicAPI]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct None :
#if NET7_0_OR_GREATER
    IEqualityOperators<None, None, bool>,
    IComparisonOperators<None, None, bool>,
#endif
    IEquatable<None>,
    IComparable<None>,
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IFormattable
{
    public static implicit operator bool(None _) => false;

    public static bool operator true(None _) => false;
    public static bool operator false(None _) => true;
 
    public static bool operator ==(None left, None right) => true;
    public static bool operator !=(None left, None right) => false;
    public static bool operator <=(None left, None right) => true;
    public static bool operator >=(None left, None right) => true;
    public static bool operator <(None left, None right) => false;
    public static bool operator >(None left, None right) => false;

    public int CompareTo(None other) => 0;

    public bool Equals(None other) => true;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is None;

    public override int GetHashCode() => 0;

    public bool TryFormat(Span<char> destination, out int charsWritten, text _ = default, IFormatProvider? __ = default)
    {
        if (destination.Length >= 4)
        {
            destination[0] = 'N';
            destination[1] = 'o';
            destination[2] = 'n';
            destination[3] = 'e';
            charsWritten = 4;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? _, IFormatProvider? __ = default) => nameof(None);

    public override string ToString() => nameof(None);
}