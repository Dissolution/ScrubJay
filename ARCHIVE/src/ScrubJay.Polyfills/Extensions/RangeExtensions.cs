namespace ScrubJay.Polyfills;

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

        public bool TryGetOffsetAndLength(int available, out (int Offset, int Length) offsetLength)
        {
            int start = range.Start.GetOffset(available);
            int end = range.End.GetOffset(available);
            if ((uint)end <= (uint)available && (uint)start <= (uint)end)
            {
                offsetLength = (start, end - start);
                return true;
            }

            offsetLength = (-1, -1);
            return false;
        }
    }

    extension(Range? optionalRange)
    {
        public bool TryGetOffsetAndLength(int available, out int offset, out int length)
        {
            if (optionalRange.IsNotNull(out var range))
                return range.TryGetOffsetAndLength(available, out offset, out length);

            offset = -1;
            length = -1;
            return false;
        }

        public bool TryGetOffsetAndLength(int available, out (int Offset, int Length) offsetLength)
        {
            if (optionalRange.IsNotNull(out var range))
                return range.TryGetOffsetAndLength(available, out offsetLength);

            offsetLength = (-1, -1);
            return false;
        }
    }
}