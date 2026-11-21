#if NET8_0_OR_GREATER
using System.Buffers;
#endif

namespace ScrubJay.Text.Splitting;

[PublicAPI]
public static class SplittingExtensions
{
    extension(string? str)
    {
        public SplitTextEnumerator SplitOn(char separator, StringComparison comparison = StringComparison.Ordinal)
        {
            if (str is null)
                return default;
            return new SplitTextEnumerator(str, separator, comparison);
        }

        public SplitTextEnumerator SplitOn(text separator, StringComparison comparison = StringComparison.Ordinal)
        {
            if (str is null)
                return default;
            return new SplitTextEnumerator(str, separator, true, comparison);
        }

        public SplitTextEnumerator SplitOnAny(text separators, StringComparison comparison = StringComparison.Ordinal)
        {
            if (str is null)
                return default;
            return new SplitTextEnumerator(str, separators, false, comparison);
        }

#if NET8_0_OR_GREATER
        public SplitTextEnumerator SplitOnAny(SearchValues<char> separators, StringComparison comparison = StringComparison.Ordinal)
        {
            if (str is null)
                return default;
            return new SplitTextEnumerator(str, separators, comparison);
        }
#endif
    }
    
    extension(text text)
    {
        public SplitTextEnumerator SplitOn(char separator, StringComparison comparison = StringComparison.Ordinal)
        {
            return new SplitTextEnumerator(text, separator, comparison);
        }

        public SplitTextEnumerator SplitOn(text separator, StringComparison comparison = StringComparison.Ordinal)
        {
            return new SplitTextEnumerator(text, separator, true, comparison);
        }

        public SplitTextEnumerator SplitOnAny(text separators, StringComparison comparison = StringComparison.Ordinal)
        {
            return new SplitTextEnumerator(text, separators, false, comparison);
        }

#if NET8_0_OR_GREATER
        public SplitTextEnumerator SplitOnAny(SearchValues<char> separators, StringComparison comparison = StringComparison.Ordinal)
        {
            return new SplitTextEnumerator(text, separators, comparison);
        }
#endif
    }
}