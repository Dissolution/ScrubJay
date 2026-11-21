namespace ScrubJay.Text.Comparison;

[PublicAPI]
public sealed class OrdinalTextComparer : StringComparisonTextComparer
{
    public OrdinalTextComparer() : base(StringComparison.Ordinal)
    {
    }

    public override bool Equals(char x, char y)
        => x == y;

    public override bool Equals(string? x, string? y)
        => string.Equals(x, y, StringComparison.Ordinal);

    public override bool Equals(text x, text y)
        => MemoryExtensions.Equals(x, y, StringComparison.Ordinal);


    public override int GetHashCode(char ch)
        => (int)ch;

    public override int GetHashCode(string? str)
    {
        if (str is null)
            return HashCode.NullHash;
        return string.GetHashCode(str, StringComparison.Ordinal);
    }

    public override int GetHashCode(text txt)
    {
        return string.GetHashCode(txt, StringComparison.Ordinal);
    }


    public override int Compare(char x, char y)
        => x.CompareTo(y);

    public override int Compare(string? x, string? y)
        => string.CompareOrdinal(x, y);

    public override int Compare(text x, text y)
        => MemoryExtensions.CompareTo(x, y, StringComparison.Ordinal);
}