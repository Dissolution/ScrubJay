namespace ScrubJay.Text.Benchmarks.BDN;

[PublicAPI]
internal sealed class StringLengthOrdinalComparer : IComparer<string?>
{
    public static StringLengthOrdinalComparer Instance { get; } = new();

    public int Compare(string? x, string? y)
    {
        if (x is not null)
        {
            if (y is not null)
            {
                int delta = x.Length - y.Length;
                if (delta != 0)
                {
                    return delta;
                }
                // ReSharper disable once InvokeAsExtensionMember
#pragma warning disable RCS1196
                return MemoryExtensions.CompareTo(x, y, StringComparison.Ordinal);
#pragma warning restore RCS1196
            }
            return 1;
        }
        if (y is not null)
        {
            return -1;
        }
        return 0;
    }
}