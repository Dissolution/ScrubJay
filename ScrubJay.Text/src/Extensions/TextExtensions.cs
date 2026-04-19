namespace ScrubJay.Text.Extensions;

/// <summary>
/// Extensions on <see cref="ReadOnlySpan{char}"/>.
/// </summary>
[PublicAPI]
public static class TextExtensions
{
    extension(scoped ReadOnlySpan<char> charSpan)
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

#region TryFindIndex
        public Option<int> TryFindIndex(char ch, bool firstToLast = true, Index? startIndex = null)
        {
            int length = charSpan.Length;
            int endIndex = length - 1;
            
            // starting index?
            if (!startIndex.TryGetOffset(length, out int offset))
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : length - 1;
            }

            // search
            if (firstToLast)
            {
                // we can scan until the last item
                for (; offset <= endIndex; offset++)
                {
                    if (charSpan[offset] == ch)
                    {
                        return Some(offset);
                    }
                }
            }
            else
            {
                // we can scan until the first item
                for (; offset >= 0; offset--)
                {
                    if (charSpan[offset] == ch)
                    {
                        return Some(offset);
                    }
                }
            }

            // no match
            return None;
        }

        public Option<int> TryFindIndex(in char ch, StringComparison comparison, bool firstToLast = true, Index? startIndex = null)
        {
            int length = charSpan.Length;
            int endIndex = length - 1;
            
            // starting index?
            if (!startIndex.TryGetOffset(length, out int offset))
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : length - 1;
            }

            // search
            text chText = ch.AsSpan();
            
            if (firstToLast)
            {
                // we can scan until the last item
                for (; offset <= endIndex; offset++)
                {
                    if (charSpan.Slice(offset, 1).Equals(chText, comparison))
                    {
                        return Some(offset);
                    }
                }
            }
            else
            {
                // we can scan until the first item
                for (; offset >= 0; offset--)
                {
                    if (charSpan.Slice(offset, 1).Equals(chText, comparison))
                    {
                        return Some(offset);
                    }
                }
            }

            // no match
            return None;
        }

        public Option<int> TryFindIndex(char ch, IEqualityComparer<char>? comparer, bool firstToLast = true, Index? startIndex = null)
        {
            if (comparer is null)
                return charSpan.TryFindIndex(ch, firstToLast, startIndex);
            
            int length = charSpan.Length;
            int endIndex = length - 1;
            
            // starting index?
            if (!startIndex.TryGetOffset(length, out int offset))
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : length - 1;
            }

            // search
            if (firstToLast)
            {
                // we can scan until the last item
                for (; offset <= endIndex; offset++)
                {
                    if (comparer.Equals(charSpan[offset], ch))
                    {
                        return Some(offset);
                    }
                }
            }
            else
            {
                // we can scan until the first item
                for (; offset >= 0; offset--)
                {
                    if (comparer.Equals(charSpan[offset], ch))
                    {
                        return Some(offset);
                    }
                }
            }

            // no match
            return None;
        }

        public Option<int> TryFindIndex(string? str, bool firstToLast = true, Index? startIndex = null)
            => charSpan.TryFindIndex(str.AsSpan(), StringComparison.Ordinal, firstToLast, startIndex);

        public Option<int> TryFindIndex(string? str, StringComparison comparison, bool firstToLast = true, Index? startIndex = null)
            => charSpan.TryFindIndex(str.AsSpan(), comparison, firstToLast, startIndex);


        public Option<int> TryFindIndex(scoped text text, bool firstToLast = true, Index? startIndex = null)
            => charSpan.TryFindIndex(text, StringComparison.Ordinal, firstToLast, startIndex);

        public Option<int> TryFindIndex(scoped text text, StringComparison comparison, bool firstToLast = true, Index? startIndex = null)
        {
            int length = charSpan.Length;
            int textLength = text.Length;
            if (textLength == 0)
                return Some(0);
            if (textLength > length)
                return None;
                
            // starting index?
            if (!startIndex.TryGetOffset(length, out int offset))
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : length - 1;
            }
            
            // we can only scan until a certain ending item
            // any further and there wouldn't be enough characters to match
            int endIndex = length - textLength;

            // clamp offset to what we can match on
            offset = Math.Clamp(offset, 0, endIndex);

            // search
            if (firstToLast)
            {
                for (; offset <= endIndex; offset++)
                {
                    if (charSpan.Slice(offset, textLength).Equals(text, comparison))
                        return Some(offset);
                }
            }
            else
            {
                for (; offset >= 0; offset--)
                {
                    if (charSpan.Slice(offset, textLength).Equals(text, comparison))
                        return Some(offset);
                }
            }

            // no match
            return None;
        }

#if NET9_0_OR_GREATER
        public Option<int> TryFindIndex(scoped text text, IEqualityComparer<text>? comparer, bool firstToLast = true, Index? startIndex = null)
        {
            if (comparer is null)
                return charSpan.TryFindIndex(text, StringComparison.Ordinal,  firstToLast, startIndex);
            
            int length = charSpan.Length;
            int textLength = text.Length;
            if (textLength == 0)
                return Some(0);
            if (textLength > length)
                return None;
                
            // starting index?
            if (!startIndex.TryGetOffset(length, out int offset))
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : length - 1;
            }
            
            // we can only scan until a certain ending item
            // any further and there wouldn't be enough characters to match
            int endIndex = length - textLength;

            // clamp offset to what we can match on
            offset = Math.Clamp(offset, 0, endIndex);

            // search
            if (firstToLast)
            {
                for (; offset <= endIndex; offset++)
                {
                    if (comparer.Equals(charSpan.Slice(offset, textLength), text))
                        return Some(offset);
                }
            }
            else
            {
                for (; offset >= 0; offset--)
                {
                    if (comparer.Equals(charSpan.Slice(offset, textLength), text))
                        return Some(offset);
                }
            }

            // no match
            return None;
        }
#endif
#endregion
    }


}