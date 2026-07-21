namespace ScrubJay.Text;

/// <summary>
/// Extensions on <see cref="ReadOnlySpan{char}"/>.
/// </summary>
[PublicAPI]
public static class TextExtensions
{
    extension(scoped text charSpan)
    {
#region Contains
#if !NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(char ch)
        {
            return charSpan.IndexOf<char>(ch) >= 0;

        }
#endif

#if !NET10_0_OR_GREATER
        public bool Contains(char ch, IEqualityComparer<char>? comparer)
        {
            if (comparer is not null)
            {
                foreach (var c in charSpan)
                {
                    if (comparer.Equals(c, ch))
                        return true;
                }
                return false;
            }
            return charSpan.Contains(ch);
        }
#endif

        public bool Contains(in char ch, StringComparison comparison)
        {
            return charSpan.Contains(ch.AsSpan(), comparison);
        }

        public bool Contains(string? str)
        {
            return charSpan.Contains(str, StringComparison.Ordinal);
        }

        public bool Contains(scoped text txt)
        {
            return charSpan.Contains(txt, StringComparison.Ordinal);
        }
#endregion /Contains

#region Indexing
        public Option<int> FindIndexOf(
            char ch,
            bool firstToLast = true,
            Range? searchRange = null,
            StringComparison comparison = StringComparison.Ordinal)
        {
            // resolve the search range
            if (!searchRange.TryGetOffsetAndLength(charSpan.Length, out int offset, out int length))
            {
                offset = 0;
                length = charSpan.Length;
            }

            int end = offset + length;

            // ordinal comparison is fast
            if (comparison == StringComparison.Ordinal)
            {
                if (firstToLast)
                {
                    for (var i = offset; i < end; i++)
                    {
                        if (charSpan[i] == ch)
                            return Some(i);
                    }
                }
                else
                {
                    for (var i = end - 1; i >= offset; i--)
                    {
                        if (charSpan[i] == ch)
                            return Some(i);
                    }
                }

            }
            else
            {
                // we have to use spans with other comparision types
                text chSpan = ch.AsSpan();

                if (firstToLast)
                {
                    for (var i = offset; i < end; i++)
                    {
                        if (charSpan.Slice(i, 1).Equals(chSpan, comparison))
                            return Some(i);
                    }
                }
                else
                {
                    for (var i = end - 1; i >= offset; i--)
                    {
                        if (charSpan.Slice(i, 1).Equals(chSpan, comparison))
                            return Some(i);
                    }
                }
            }
            return default;
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
            if (!searchRange.TryGetOffsetAndLength(charSpan.Length, out int offset, out int length))
            {
                offset = 0;
                length = charSpan.Length;
            }

            if (firstToLast)
            {
                var i = charSpan.Slice(offset, length).IndexOf(subtext, comparison);

                if (i >= 0)
                {
                    return Some(i + offset);
                }
            }
            else
            {
#if NET6_0_OR_GREATER
                var i = charSpan.Slice(offset, length).LastIndexOf(subtext, comparison);
#else
                int i;
                if (comparison == StringComparison.Ordinal)
                {
                    i = charSpan.Slice(offset, length).LastIndexOf(subtext);
                }
                else
                {
                    var f = subtext[0];
                    var slice = charSpan.Slice(offset, length);
                    for (i = slice.Length - subtextLength; i >= 0; i--)
                    {
                        if (subtext[i] == f && subtext.Slice(i, subtextLength).Equals(subtext, comparison))
                        {
                            goto fin;
                        }
                    }
                    return default;
                }

fin:
#endif

                if (i >= 0)
                {
                    return Some(i + offset);
                }
            }

            return default;
        }

        public Option<int> FindIndexOfAny(
            scoped text characters,
            bool firstToLast = true,
            Range? searchRange = null,
            StringComparison comparison = StringComparison.Ordinal)
        {
            throw new NotImplementedException();
        }
#endregion
    }
}