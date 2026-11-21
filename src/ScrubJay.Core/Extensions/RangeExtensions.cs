using System.Security.Principal;

namespace ScrubJay.Extensions;

[PublicAPI]
public static class IndexExtensions
{
    extension(Index)
    {
        public static bool operator ==(Index left, Index right) => left.Equals(right);
        public static bool operator !=(Index left, Index right) => !left.Equals(right);
    }

    extension(Index index)
    {
        public bool IsUnbounded => index == Index.Start || index == Index.End;
    }
}

public enum RangeFormat
{
    Default,
    Interval,
    Declaration,
}

[PublicAPI]
public static class RangeExtensions
{
    extension(Range range)
    {
        public string ToString(RangeFormat format)
        {
            switch (format)
            {
                case RangeFormat.Interval:
                {
                    return $"[{range.Start}, {range.End})";
                }
                case RangeFormat.Declaration:
                {
                    return $"{range.Start}..{range.End}";
                }
                case RangeFormat.Default:
                default:
                {
                    return range.ToString();
                }
            }
        }
    }
}