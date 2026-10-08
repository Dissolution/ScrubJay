// public static unsafe bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer = null)
//        {
//            // If the spans differ in length, they're not equal.
//            if (span.Length != other.Length)
//            {
//                return false;
//            }
//
//            if (typeof(T).IsValueType)
//            {
//                if (comparer is null || comparer == EqualityComparer<T>.Default)
//                {
//                    // If no comparer was supplied and the type is bitwise equatable, take the fast path doing a bitwise comparison.
//                    if (RuntimeHelpers.IsBitwiseEquatable<T>())
//                    {
//                        return SpanHelpers.SequenceEqual(
//                            ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)),
//                            ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(other)),
//                            ((uint)span.Length) * (nuint)sizeof(T));  // If this multiplication overflows, the Span we got overflows the entire address range. There's no happy outcome for this API in such a case so we choose not to take the overhead of checking.
//                    }
//
//                    // Otherwise, compare each element using EqualityComparer<T>.Default.Equals in a way that will enable it to devirtualize.
//                    for (int i = 0; i < span.Length; i++)
//                    {
//                        if (!EqualityComparer<T>.Default.Equals(span[i], other[i]))
//                        {
//                            return false;
//                        }
//                    }
//
//                    return true;
//                }
//            }
//
//            // Use the comparer to compare each element.
//            comparer ??= EqualityComparer<T>.Default;
//            for (int i = 0; i < span.Length; i++)
//            {
//                if (!comparer.Equals(span[i], other[i]))
//                {
//                    return false;
//                }
//            }
//
//            return true;
////        }a