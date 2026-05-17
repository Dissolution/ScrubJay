using ScrubJay.Text.Comparison;

namespace ScrubJay.Text.Extensions;

/// <summary>
/// Extensions on <see cref="ReadOnlySpan{T}"/>.
/// </summary>
[PublicAPI]
public static class TextExtensions
{
    private static (int start, int end) Resolve(bool firstToLast, Index? startIndex, int available, int matchLength)
    {
        if (!startIndex.TryGetOffset(available, out int start))
        {
            start = firstToLast ? 0 : available - 1;
        }

        int end = available - matchLength;
        return (start, end);
    }

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
    }

    extension(text text)
    {
        public SplitTextEnumerator SplitOn(char separator, TextComparison? comparison = null)
        {
            return new SplitTextEnumerator(text, separator, comparison);
        }

        public SplitTextEnumerator SplitOn(text separator, TextComparison? comparison = null)
        {
            return new SplitTextEnumerator(text, separator, true, comparison);
        }

        public SplitTextEnumerator SplitOnAny(text separators, TextComparison? comparison = null)
        {
            return new SplitTextEnumerator(text, separators, false, comparison);
        }

#if NET8_0_OR_GREATER
        public SplitTextEnumerator SplitOnAny(SearchValues<char> separators, TextComparison? comparison = null)
        {
            return new SplitTextEnumerator(text, separators, comparison);
        }
#endif
    }

}