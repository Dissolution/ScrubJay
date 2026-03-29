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
            return Ex.ArgRange(in range, $"[0..{available})");
        }

        return (start, end - start);
    }

    public static Result<(int Start, int Length)> Range(Index index, int length, int available)
    {
        if (available < 0)
            return Ex.ArgRange(in available, i => i >= 0);

        int start = index.GetOffset(available);

        if (Environment.Is64BitProcess)
        {
            if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)available)
                return Ex.ArgRange((index, length), $"[0..{available})");
        }
        else
        {
            if ((uint)start > (uint)available)
                return Ex.ArgRange(in index, $"[0..{available}]");
            if ((uint)length > (uint)(available - start))
                return Ex.ArgRange(in length, $"[0..{available - start}]");
        }

        return (start, length);
    }
}

public static partial class Throw
{
    public static void IfBadRange(int start, int length, int available)
    {
        if (available < 0)
            throw Ex.ArgRange(in available, i => i >= 0);

        if (Environment.Is64BitProcess)
        {
            if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)available)
                throw Ex.ArgRange((start, length), $"[0..{available})");
        }
        else
        {
            if ((uint)start > (uint)available)
                throw Ex.ArgRange(in start, $"[0..{available}]");
            if ((uint)length > (uint)(available - start))
                throw Ex.ArgRange(in length, $"[0..{available - start}]");
        }
    }
}