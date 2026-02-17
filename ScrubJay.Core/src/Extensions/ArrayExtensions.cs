namespace ScrubJay.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension(Array? array)
    {
        public Type? ElementType => array?.GetType().GetElementType();
    }

    /// <summary>
    /// Returns <c>true</c> if <paramref name="array"/> is <c>null</c> or has a Length of 0
    /// </summary>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this Array? array)
        => array is null || (array.Length == 0);


    extension<T>(T[]? array)
    {
        public ref T FirstItemRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (array is null)
                    return ref Notsafe.NullRef<T>();
#if !NETSTANDARD
                return ref MemoryMarshal.GetArrayDataReference(array);
#else
                if (array.Length == 0)
                {
                    unsafe
                    {
                        return ref Unsafe.AsRef<T>(null);
                    }
                }

                return ref array[0];
#endif
            }
        }

        public ref readonly T FirstItemReadonlyRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (array is null)
                    return ref Notsafe.NullRef<T>();
#if !NETSTANDARD
                return ref MemoryMarshal.GetArrayDataReference(array);
#else
                if (array.Length == 0)
                {
                    unsafe
                    {
                        return ref Unsafe.AsRef<T>(null);
                    }
                }

                return ref array[0];
#endif
            }
        }

        public bool TryGetAt(int index, [MaybeNullWhen(false)] out T value)
        {
            if (array is not null)
            {
                if ((uint)index < (uint)array.Length)
                {
                    value = array[index];
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetAt(Index index, [MaybeNullWhen(false)] out T value)
        {
            if (array is not null)
            {
                int length = array.Length;
                int offset = index.GetOffset(length);

                if ((uint)offset < (uint)length)
                {
                    value = array[offset];
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TrySetAt(int index, T value)
        {
            if (array is not null)
            {
                if ((uint)index < (uint)array.Length)
                {
                    array[index] = value;
                    return true;
                }
            }

            return false;
        }

        public bool TrySetAt(Index index, T value)
        {
            if (array is not null)
            {
                int length = array.Length;
                int offset = index.GetOffset(length);

                if ((uint)offset < (uint)length)
                {
                    array[offset] = value;
                    return true;
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
            if (array is null)
                return None;

            int index;
            int offset;

            if (itemComparer is null)
            {
                if (startIndex.TryGetValue(out var start))
                {
                    offset = start.GetOffset(array.Length);
                    if (firstToLast)
                    {
                        index = Array.IndexOf<T>(array, item, offset);
                    }
                    else
                    {
                        index = Array.LastIndexOf<T>(array, item, offset);
                    }
                }
                else
                {
                    if (firstToLast)
                    {
                        index = Array.IndexOf<T>(array, item);
                    }
                    else
                    {
                        index = Array.LastIndexOf<T>(array, item);
                    }
                }

                return Option<int>.SomeIf(index, static i => i >= 0);
            }

            //else
            {
                int end = array.Length - 1;

                if (startIndex.TryGetValue(out var start))
                {
                    offset = start.GetOffset(array.Length);
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
                        if (itemComparer.Equals(array[offset], item))
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
                        if (itemComparer.Equals(array[offset], item))
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
            if (array is not null && indexedItemRef is not null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    indexedItemRef(ref array[i], i);
                }
            }
        }

        public bool Contains(T item)
        {
            if (array is null) return false;
            return Array.IndexOf<T>(array, item) >= 0;
        }

#if NETFRAMEWORK || NETSTANDARD2_0
        public Span<T> AsSpan(Range range)
        {
            if (array is null)
                return [];
            (int start, int length) = range.GetOffsetAndLength(array.Length);
            return new Span<T>(array, start, length);
        }
#endif

        [return: NotNullIfNotNull(nameof(array))]
        public O[]? SelectToArray<O>(Converter<T, O> selector)
        {
            if (array is null)
                return null;
            return Array.ConvertAll<T, O>(array, selector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] SubArray(int start) => array.AsSpan(start).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] SubArray(int start, int length) => array.AsSpan(start, length).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] SubArray(Range range) => array.AsSpan(range).ToArray();

#if !NET10_0_OR_GREATER
        public void Reverse()
        {
            if (array is not null)
            {
#if !NETSTANDARD2_0
                Array.Reverse<T>(array);
#else
                int len = array.Length;
                if (len > 1)
                {

                    ref T first = ref array.FirstItemRef;
                    ref T last = ref Unsafe.Subtract(ref Unsafe.Add(ref first, len), 1);
                    do
                    {
                        T temp = first;
                        first = last;
                        last = temp;
                        first = ref Unsafe.Add(ref first, 1);
                        last = ref Unsafe.Subtract(ref last, 1);
                    } while (Unsafe.IsAddressLessThan(ref first, ref last));
                }
#endif
            }
        }
#endif
    }

    /// <summary>
    /// Returns <c>true</c> if <paramref name="array"/> is <c>null</c> or has a Length of 0
    /// </summary>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this T[]? array)
        => array is null || (array.Length == 0);
}