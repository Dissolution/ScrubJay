namespace ScrubJay.Functional;

[PublicAPI]
public static class RangeExtensions
{
    extension(Range range)
    {
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

    extension(Range? optionalRange)
    {
        public Option<(int Offset, int Length)> TryGetOffsetAndLength(int available)
        {
            if (optionalRange.IsNotNull(out var range))
                return range.TryGetOffsetAndLength(available);
            return default;
        }
    }

    extension(Option<Range> optionalRange)
    {
        public bool TryGetOffsetAndLength(int available, out int offset, out int length)
        {
            if (optionalRange.IsSome(out var range))
                return range.TryGetOffsetAndLength(available, out offset, out length);

            offset = -1;
            length = -1;
            return false;
        }

        public bool TryGetOffsetAndLength(int available, out (int Offset, int Length) offsetLength)
        {
            if (optionalRange.IsSome(out var range))
                return range.TryGetOffsetAndLength(available, out offsetLength);

            offsetLength = (-1, -1);
            return false;
        }

        public Option<(int Offset, int Length)> TryGetOffsetAndLength(int available)
        {
            if (optionalRange.IsSome(out var range))
                return range.TryGetOffsetAndLength(available);
            return default;
        }
    }
}