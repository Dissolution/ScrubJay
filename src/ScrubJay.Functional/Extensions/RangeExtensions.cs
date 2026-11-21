namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class RangeExtensions
{
    extension(Range range)
    {
        public bool TryGetOffsetAndLength(int available, out int offset, out int length)
        {
            int start = range.Start.GetOffset(available);
            int end = range.End.GetOffset(available);

            if ((uint)end <= (uint)available && (uint)start <= (uint)end)
            {
                offset = start;
                length = end - start;
                return true;
            }

            offset = -1;
            length = -1;
            return false;
        }

        public Option<(int Offset, int Length)> TryGetOffsetAndLength(int available)
        {
            int start = range.Start.GetOffset(available);
            int end = range.End.GetOffset(available);

            if ((uint)end <= (uint)available && (uint)start <= (uint)end)
            {
                return (start, end - start);
            }

            return default;
        }
    }

    extension(Nullable<Range> optionalRange)
    {
        public bool TryGetOffsetAndLength(int available, out int offset, out int length)
        {
            if (!optionalRange.TryGetValue(out var range))
            {
                offset = 0;
                length = available;
                return true;
            }

            return range.TryGetOffsetAndLength(available, out offset, out length);
        }

        public Option<(int Offset, int Length)> TryGetOffsetAndLength(int available)
        {
            if (!optionalRange.TryGetValue(out var range))
                return (0, available);
            return range.TryGetOffsetAndLength(available);
        }
    }
}