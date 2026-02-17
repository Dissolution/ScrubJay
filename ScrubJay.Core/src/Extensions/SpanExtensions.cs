#pragma warning disable S3776, MA0051

namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on <see cref="Span{T}"/>
/// </summary>
[PublicAPI]
public static class SpanExtensions
{
    // unscoped extensions
    extension<T>(Span<T> span)
    {
        public ref T FirstItemRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref span.GetPinnableReference();
        }

        public ref readonly T FirstItemReadonlyRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref span.GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(int index)
        {
            if ((uint)index < (uint)span.Length)
            {
                // skip extra bounds check
                return ref Notsafe.OffsetRef(ref span.GetPinnableReference(), (nint)(uint)index);
            }

            return ref Notsafe.NullRef<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(Index index)
        {
            int length = span.Length;
            int offset = index.GetOffset(length);

            if ((uint)offset < (uint)length)
            {
                // skip extra bounds check
                return ref Notsafe.OffsetRef(ref span.GetPinnableReference(), (nint)(uint)offset);
            }

            return ref Notsafe.NullRef<T>();
        }

        public bool TrySlice(Range range, out Span<T> slice)
        {
            slice = span[range];
            return true;
        }
    }

    // scoped extensions
    extension<T>(scoped Span<T> span)
    {
#region Item Get/Set

        #region TryGetAt
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
        public Option<T> TryGetAt(int index)
        {
            if ((uint)index < (uint)span.Length)
            {
                // skip extra bounds check
                var value = Notsafe.OffsetReadOnlyRef(in span.GetPinnableReference(), (nint)(uint)index);
                return Option<T>.Some(value);
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> TryGetAt(Index index)
        {
            int length = span.Length;
            int offset = index.GetOffset(length);

            if ((uint)offset < (uint)length)
            {
                // skip extra bounds check
                var value = Notsafe.OffsetReadOnlyRef(in span.GetPinnableReference(), (nint)(uint)offset);
                return Option<T>.Some(value);
            }

            return default;
        }
        
        #endregion
        
        #region TrySetAt

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
        #endregion
        

#endregion

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

        public void ForEach(ActRef<T>? itemRef)
        {
            if (itemRef is not null)
            {
                for (int i = 0; i < span.Length; i++)
                {
                    itemRef(ref span[i]);
                }
            }
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
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(scoped ReadOnlySpan<T> other) => other.CopyTo(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(scoped Span<T> other) => other.CopyTo(span);
    }





    public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> slice, IEqualityComparer<T>? itemComparer)
    {
        int sliceLen = slice.Length;
        if (sliceLen > span.Length)
            return false;
        return Sequence.Equal<T>(span[..sliceLen], slice, itemComparer);
    }

    public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> slice,
        IEqualityComparer<T>? itemComparer)
    {
        int sliceLen = slice.Length;
        if (sliceLen > span.Length)
            return false;
        return Sequence.Equal<T>(span[..sliceLen], slice, itemComparer);
    }

    /// <summary>
    /// Does this <see cref="Span{T}"/> contain the given <paramref name="item"/> as determined by an <see cref="IEqualityComparer{T}"/>?
    /// </summary>
    /// <param name="span"></param>
    /// <param name="item"></param>
    /// <param name="itemComparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Contains<T>(this Span<T> span, T item, IEqualityComparer<T>? itemComparer)
    {
        int spanLen = span.Length;
        itemComparer ??= EqualityComparer<T>.Default;
        for (int i = 0; i < spanLen; i++)
        {
            if (itemComparer.Equals(span[i], item))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Does this <see cref="ReadOnlySpan{T}"/> contain the given <paramref name="item"/> as determined by an <see cref="IEqualityComparer{T}"/>?
    /// </summary>
    /// <param name="span"></param>
    /// <param name="item"></param>
    /// <param name="itemComparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Contains<T>(this ReadOnlySpan<T> span, T item, IEqualityComparer<T>? itemComparer)
    {
        int spanLen = span.Length;
        itemComparer ??= EqualityComparer<T>.Default;
        for (int i = 0; i < spanLen; i++)
        {
            if (itemComparer.Equals(span[i], item))
                return true;
        }

        return false;
    }
    /*
       public static SpanSplitter<T> Splitter<T>(this ReadOnlySpan<T> span, T separator,
           SpanSplitOptions options = SpanSplitOptions.None)
           where T : IEquatable<T>
           => SpanSplitter<T>.Split(span, separator, options);

       public static SpanSplitter<T> Splitter<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separator,
           SpanSplitOptions options = SpanSplitOptions.None)
           where T : IEquatable<T>
           => SpanSplitter<T>.Split(span, separator, options);

       public static SpanSplitter<T> SplitterAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separators,
           SpanSplitOptions options = SpanSplitOptions.None)
           where T : IEquatable<T>
           => SpanSplitter<T>.SplitAny(span, separators, options);
           */
}