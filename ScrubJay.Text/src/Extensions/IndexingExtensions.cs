namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class IndexingExtensions
{
    extension(scoped text text)
    {
        public Option<int> FindIndexOf(
            char ch,
            bool firstToLast = true,
            Range? searchRange = null,
            StringComparison comparison = StringComparison.Ordinal)
        {
            // resolve the search range
            if (!searchRange.TryGetOffsetAndLength(text.Length, out int offset, out int length))
            {
                offset = 0;
                length = text.Length;
            }

            int end = offset + length;

            // ordinal comparison is fast
            if (comparison == StringComparison.Ordinal)
            {
                if (firstToLast)
                {
                    for (var i = offset; i < end; i++)
                    {
                        if (text[i] == ch)
                            return Some(i);
                    }
                }
                else
                {
                    for (var i = end - 1; i >= offset; i--)
                    {
                        if (text[i] == ch)
                            return Some(i);
                    }
                }

                return None;
            }

            // we have to use spans with other comparision types
            text chSpan = ch.AsSpan();
            Debug.Assert(chSpan.Length == 1 && chSpan[0] == ch);

            if (firstToLast)
            {
                for (var i = offset; i < end; i++)
                {
                    if (text.Slice(i, 1).Equals(chSpan, comparison))
                        return Some(i);
                }
            }
            else
            {
                for (var i = end - 1; i >= offset; i--)
                {
                    if (text.Slice(i, 1).Equals(chSpan, comparison))
                        return Some(i);
                }
            }

            return None;
        }

        public Option<int> FindIndexOf(
            scoped text subtext,
            bool firstToLast = true,
            Range? searchRange = null,
            StringComparison comparison = StringComparison.Ordinal)
        {
            int subtextLength = subtext.Length;
            if (subtextLength == 0)
                return Some(0);

            // resolve the search range
            if (!searchRange.TryGetOffsetAndLength(text.Length, out int offset, out int length))
            {
                offset = 0;
                length = text.Length;
            }

            if (firstToLast)
            {
                var i = text.Slice(offset, length).IndexOf(subtext, comparison);

                if (i >= 0)
                {
                    return Some(i + offset);
                }
            }
            else
            {
#if NET6_0_OR_GREATER
                var i = text.Slice(offset, length).LastIndexOf(subtext, comparison);
#else
                int i;
                if (comparison == StringComparison.Ordinal)
                {
                    i = text.Slice(offset, length).LastIndexOf(subtext);
                }
                else
                {
                    var f = subtext[0];
                    var slice = text.Slice(offset, length);
                    for (i = slice.Length - subtextLength; i >= 0; i--)
                    {
                        if (subtext[i] == f && subtext.Slice(i, subtextLength).Equals(subtext, comparison))
                        {
                            goto fin;
                        }
                    }
                    return None;
                }

                fin:
#endif

                if (i >= 0)
                {
                    return Some(i + offset);
                }
            }

            return None;
        }

        public Option<int> FindIndexOfAny(
            scoped text characters,
            bool firstToLast = true,
            Range? searchRange = null,
            StringComparison comparison = StringComparison.Ordinal)
        {
            throw new NotImplementedException();
        }
    }
}