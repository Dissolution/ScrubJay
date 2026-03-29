namespace ScrubJay.Enhancements.Validation;

public static partial class Validate
{
    public static Result<(int Start, int Length)> Range(Range range, int available)
    {
        if (available < 0)
            return Ex.ArgRange(in available, i => i >= 0);
        
        int start = range.Start.GetOffset(available);
        int end = range.End.GetOffset(available);

        if ((uint)end > (uint)available || (uint)start > (uint)end)
        {
            return Ex.ArgRange(in range, $"[0..{available}]");
        }

        return (start, end - start);
    }
}