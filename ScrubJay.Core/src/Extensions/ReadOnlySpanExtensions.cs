namespace ScrubJay.Extensions;

[PublicAPI]
public static class ReadOnlySpanExtensions
{
    // unscoped
    extension<T>(ReadOnlySpan<T> span)
    {
        public ref readonly T FirstItemReadonlyRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref span.GetPinnableReference();
        }
    }

    // scoped
    extension<T>(scoped ReadOnlySpan<T> span)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetAt(int index, [MaybeNullWhen(false)] out T value)
        {
            if ((uint)index < (uint)span.Length)
            {
                // skip extra bounds check
                value = Notsafe.OffsetReadOnlyRef(in span.GetPinnableReference(), (nint)(uint)index);
                return true;
            }

            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetAt(Index index, [MaybeNullWhen(false)] out T value)
        {
            int length = span.Length;
            int offset = index.GetOffset(length);

            if ((uint)offset < (uint)length)
            {
                // skip extra bounds check
                value = Notsafe.OffsetReadOnlyRef(in span.GetPinnableReference(), (nint)(uint)offset);
                return true;
            }

            value = default;
            return false;
        }
        
        public Option<int> TryFindIndex(
            T item,
            IEqualityComparer<T>? itemComparer = null,
            bool firstToLast = true,
            Index? startIndex = null)
        {
            int len = span.Length;

            int offset;
            if (startIndex.TryGetValue(out var start))
            {
                offset = start.GetOffset(len);
            }
            else
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : len - 1;
            }

            itemComparer ??= EqualityComparer<T>.Default;

            if (firstToLast)
            {
                // we can scan until the last item
                for (; offset < len; offset++)
                {
                    if (itemComparer.Equals(span[offset], item))
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
                    if (itemComparer.Equals(span[offset], item))
                    {
                        return Some(offset);
                    }
                }
            }

            // failed to find
            return None;
        }
        
        public Option<int> TryFindIndex(
            scoped ReadOnlySpan<T> slice,
            IEqualityComparer<T>? itemComparer = null,
            bool firstToLast = true,
            Index? startIndex = null)
        {
            int searchLen = span.Length;
            int sliceLen = slice.Length;
            int end = searchLen - sliceLen; // inclusive

            int offset;
            if (startIndex.TryGetValue(out var start))
            {
                offset = start.GetOffset(searchLen);
            }
            else
            {
                // first-to-last: start at first item
                // last-to-first: start at the last item
                offset = firstToLast ? 0 : searchLen - 1;
            }

            itemComparer ??= EqualityComparer<T>.Default;

            throw Ex.NotImplemented();
            /*
            
            if (firstToLast)
            {
                // we can scan until the last item
                for (; offset < searchLen; offset++)
                {
                    if (itemComparer.Equals(span[offset], slice))
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
                    if (itemComparer.Equals(span[offset], slice))
                    {
                        return Some(offset);
                    }
                }
            }

            // failed to find
            return None;
            */
        }

        public Option<int> TryFindIndexOfAny(
            scoped ReadOnlySpan<T> items,
            IEqualityComparer<T>? itemComparer = null,
            bool firstToLast = true,
            Index? startIndex = null)
        {
            throw Ex.NotImplemented();
        }
    }

}