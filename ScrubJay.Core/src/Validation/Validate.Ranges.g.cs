#nullable enable

namespace ScrubJay.Validation;


partial class Validate
{
    public static Result<(int Offset, int Length)> Range(Range range, int available,
        [CallerArgumentExpression(nameof(range))] string? rangeName = null)
    {
        int start = range.Start.GetOffset(available);
        int end = range.End.GetOffset(available);

        if ((uint)end > (uint)available || (uint)start > (uint)end)
        {
            return Ex.ArgRange(range, $"did not fit in [{available}]", rangeName);
        }

        return (start, end - start);
    }

    public static Result<(int Offset, int Length)> Range(Index start, int length, int available,
        [CallerArgumentExpression(nameof(start))] string? startName = null,
        [CallerArgumentExpression(nameof(length))] string? lengthName = null)
    {
        int startIndex = start.GetOffset(available);
        if ((uint)startIndex + (uint)length > (uint)available)
        {
            return Ex.ArgRange((start,length), $"did not fit in [{available}]", $"{startName}+{lengthName}");
        }

        return (startIndex, length);
    }

    public static Result<(int Offset, int Length)> Range(int start, int length, int available,
        [CallerArgumentExpression(nameof(start))] string? startName = null,
        [CallerArgumentExpression(nameof(length))] string? lengthName = null)
    {
        if ((uint)start + (uint)length > (uint)available)
        {
            return Ex.ArgRange((start,length), $"did not fit in [{available}]", $"{startName}+{lengthName}");
        }

        return (start, length);
    }
}
