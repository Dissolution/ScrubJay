#if !NET8_0_OR_GREATER

namespace ScrubJay.Polyfills;

public static partial class PolyfillExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer = null) =>
        span.IndexOf(value, comparer) >= 0;
    
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer = null)
        {
            if (typeof(T).IsValueType && (comparer is null || ReferenceEquals(comparer, EqualityComparer<T>.Default)))
            {
                return IndexOfDefaultComparer(span, value);
                static int IndexOfDefaultComparer(ReadOnlySpan<T> span, T value)
                {
                    for (int i = 0; i < span.Length; i++)
                    {
                        if (EqualityComparer<T>.Default.Equals(span[i], value))
                        {
                            return i;
                        }
                    }

                    return -1;
                }
            }
            else
            {
                return IndexOfComparer(span, value, comparer);
                static int IndexOfComparer(ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer)
                {
                    comparer ??= EqualityComparer<T>.Default;
                    for (int i = 0; i < span.Length; i++)
                    {
                        if (comparer.Equals(span[i], value))
                        {
                            return i;
                        }
                    }

                    return -1;
                }
            }
        }
}


#endif