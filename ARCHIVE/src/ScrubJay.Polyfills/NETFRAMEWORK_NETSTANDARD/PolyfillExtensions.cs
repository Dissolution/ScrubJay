#if NETFRAMEWORK || NETSTANDARD

using ScrubJay.Errors.Validation;


namespace ScrubJay.Polyfills
{
    public static partial class PolyfillExtensions
    {
        extension(MemoryMarshal)
        {
            /// <summary>
            /// Returns a reference to the 0th element of <paramref name="array"/>. If the array is empty, returns a reference to where the 0th element
            /// would have been stored. Such a reference may be used for pinning but must never be dereferenced.
            /// </summary>
            /// <exception cref="NullReferenceException"><paramref name="array"/> is <see langword="null"/>.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ref T GetArrayDataReference<T>(T[] array)
            {
                Throw.IfNull(array);
                return ref MemoryMarshal.GetReference(array.AsSpan());
            }
        }

        extension(MemoryExtensions)
        {
            public static bool SequenceEqual<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer = null)
            {
                // If the spans differ in length, they're not equal.
                if (span.Length != other.Length)
                {
                    return false;
                }

                if (typeof(T).IsValueType)
                {
                    if (comparer is null || EqualityComparer<T>.Default.Equals(comparer))
                    {
                        // Compare each element using EqualityComparer<T>.Default.Equals in a way that will enable it to devirtualize.
                        for (int i = 0; i < span.Length; i++)
                        {
                            if (!EqualityComparer<T>.Default.Equals(span[i], other[i]))
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }

                // Use the comparer to compare each element.
                comparer ??= EqualityComparer<T>.Default;
                for (int i = 0; i < span.Length; i++)
                {
                    if (!comparer.Equals(span[i], other[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}

#endif