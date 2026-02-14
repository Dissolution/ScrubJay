using System.Collections.Specialized;

namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on indexable collections
/// </summary>
[PublicAPI]
public static class IndexableCollectionExtensions
{
    

    
#region Span<T>

    extension<T>(Span<T> span)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetRef()
        {
            return ref span.GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T GetReadonlyRef()
        {
            return ref span.GetPinnableReference();
        }
    }

    extension<T>(scoped Span<T> span)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetAt(int index, T value)
        {
            if ((uint)index < (uint)span.Length)
            {
                // skip extra bounds check
                Notsafe.OffsetRef(ref span.GetPinnableReference(), (nint)(uint)index) = value;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetAt(Index index, T value)
        {
            int length = span.Length;
            int offset = index.GetOffset(length);

            if ((uint)offset < (uint)length)
            {
                // skip extra bounds check
                Notsafe.OffsetRef(ref span.GetPinnableReference(), (nint)(uint)offset) = value;
                return true;
            }

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

        public void ForEach(IndexedItemRef<T>? indexedItemRef)
        {
            if (indexedItemRef is not null)
            {
                for (int i = 0; i < span.Length; i++)
                {
                    indexedItemRef(ref span[i], i);
                }
            }
        }
    }

#endregion

#region ReadOnlySpan<T>

    extension<T>(ReadOnlySpan<T> span)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T GetReadonlyRef()
        {
            return ref span.GetPinnableReference();
        }
    }

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
    }

#endregion
}