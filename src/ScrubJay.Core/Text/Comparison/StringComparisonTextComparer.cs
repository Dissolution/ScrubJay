namespace ScrubJay.Text.Comparison;

[PublicAPI]
public abstract class StringComparisonTextComparer : TextComparer
{
    public StringComparison StringComparison { get; }

    protected StringComparisonTextComparer(StringComparison stringComparison)
    {
        StringComparison = stringComparison;
    }
}