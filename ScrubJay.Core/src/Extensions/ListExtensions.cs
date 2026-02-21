namespace ScrubJay.Extensions;

[PublicAPI]
public static class ListExtensions
{
    extension<T>(List<T>? list)
    {
#if !(NETSTANDARD || NETFRAMEWORK)
        public ref T FirstItemRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref CollectionsMarshal.AsSpan(list).FirstItemRef;
        }


        public ref readonly T FirstItemReadonlyRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref CollectionsMarshal.AsSpan(list).FirstItemReadonlyRef;
        }
#endif

        public bool TryGetAt(int index, [MaybeNullWhen(false)] out T value)
        {
            if (list is not null)
            {
                if ((uint)index < (uint)list.Count)
                {
                    value = list[index];
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetAt(Index index, [MaybeNullWhen(false)] out T value)
        {
            if (list is not null)
            {
                int length = list.Count;
                int offset = index.GetOffset(length);

                if ((uint)offset < (uint)length)
                {
                    value = list[offset];
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TrySetAt(int index, T value)
        {
            if (list is not null)
            {
                if ((uint)index < (uint)list.Count)
                {
                    list[index] = value;
                }
            }

            return false;
        }

        public bool TrySetAt(Index index, T value)
        {
            if (list is not null)
            {
                int length = list.Count;
                int offset = index.GetOffset(length);

                if ((uint)offset < (uint)length)
                {
                    list[offset] = value;
                }
            }

            return false;
        }

        public Option<int> TryFindIndex(
            T item,
            IEqualityComparer<T>? itemComparer = null,
            bool firstToLast = true,
            Index? startIndex = null)
        {
            if (list is null)
                return None;

            int index;
            int offset;

            if (itemComparer is null)
            {
                if (startIndex.TryGetValue(out var start))
                {
                    offset = start.GetOffset(list.Count);
                    if (firstToLast)
                    {
                        index = list.IndexOf(item, offset);
                    }
                    else
                    {
                        index = list.LastIndexOf(item, offset);
                    }
                }
                else
                {
                    if (firstToLast)
                    {
                        index = list.IndexOf(item);
                    }
                    else
                    {
                        index = list.LastIndexOf(item);
                    }
                }

                return Option<int>.SomeIf(index, static i => i >= 0);
            }

            //else
            {
                int end = list.Count - 1;

                if (startIndex.TryGetValue(out var start))
                {
                    offset = start.GetOffset(list.Count);
                }
                else
                {
                    // first-to-last: start at first item
                    // last-to-first: start at the last item
                    offset = firstToLast ? 0 : end;
                }

                if (firstToLast)
                {
                    // we can scan until the last item
                    for (; offset <= end; offset++)
                    {
                        if (itemComparer.Equals(list[offset], item))
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
                        if (itemComparer.Equals(list[offset], item))
                        {
                            return Some(offset);
                        }
                    }
                }

                // failed to find
                return None;
            }
        }

        public void ForEach(IndexedItemRef<T>? indexedItemRef)
        {
            if (list is not null && indexedItemRef is not null)
            {
                T temp;

                for (int i = 0; i < list.Count; i++)
                {
                    temp = list[i];
                    indexedItemRef(ref temp, i);
                    list[i] = temp;
                }
            }
        }
    }
    
    /// <summary>
    /// Returns <c>true</c> if <paramref name="list"/> is <c>null</c> or has a Length of 0
    /// </summary>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this List<T>? list)
        => list is null || (list.Count == 0);
}