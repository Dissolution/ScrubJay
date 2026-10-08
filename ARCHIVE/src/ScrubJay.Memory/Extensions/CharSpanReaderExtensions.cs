namespace ScrubJay.Memory;

public static class CharSpanReaderExtensions
{
    extension(ref SpanReader<char> textReader)
    {
#region (Try)TakeManyToString
        public bool TryTakeManyToString(int count, [NotNullWhen(true)] out string? taken)
        {
            if (textReader.TryTakeMany(count, out var slice))
            {
                taken = slice.ToString();
                return true;
            }
            taken = null;
            return false;
        }
    
        public string TakeManyToString(int count) => textReader.TakeMany(count).ToString();
#endregion

        public text TakeWhileMatching(scoped text match, StringComparison comparison = StringComparison.Ordinal)
        {
            int matchLen = match.Length;
            if (matchLen == 0)
                return [];

            var (span, length, start) = textReader;
            int index = start;

            while (index < length && span.Slice(index, matchLen).Equals(match, comparison))
            {
                index += matchLen;
            }

            textReader._position = index;
            return span[start..index];
        }
        
        public text TakeUntilMatching(
            scoped text match,
            StringComparison comparison = StringComparison.Ordinal,
            bool chunk = false)
        {
            int matchLen = match.Length;
            if (matchLen == 0)
                return [];

            var (span, len, start) =  textReader;
            int index = start;
            while (index < len && !span.Slice(index, matchLen).Equals(match, comparison))
            {
                if (chunk)
                    index += matchLen;
                else
                    index++;
            }

            textReader._position = index;
            return span[start..index];
        }
    }
}