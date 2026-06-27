using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class HashCodeExtensions
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Add")]
    private static extern void HashCodeAddInt32Hash(ref HashCode hashCode, int hash);
#else
#pragma warning disable IDE1006
    private static readonly UnsafeAccessor.MethodAction<HashCode, int> HashCodeAddInt32Hash
#pragma warning restore IDE1006
        = UnsafeAccessor.GetMethodInvoker<HashCode, int>("Add");
#endif

    private static readonly int _nullHash = HashCode.Combine<object?>(null);
    private static readonly int _emptyHash = new HashCode().ToHashCode();

    extension(HashCode)
    {
        /// <summary>
        /// The current hashcode for <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// All <see langword="null"/> values will produce this hashcode.
        /// </remarks>
        public static int NullHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _nullHash;
        }

        /// <summary>
        /// The current hashcode for an empty collection.
        /// </summary>
        /// <remarks>
        /// All empty collections will produce this hashcode.
        /// </remarks>
        public static int EmptyHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _emptyHash;
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="Span{T}"/>.
        /// </summary>
        public static int Combine<T>(scoped Span<T> span)
        {
            switch (span.Length)
            {
                case 0:
                    return _emptyHash;
                case 1:
                    return HashCode.Combine(span[0]);
                case 2:
                    return HashCode.Combine(span[0], span[1]);
                case 3:
                    return HashCode.Combine(span[0], span[1], span[2]);
                case 4:
                    return HashCode.Combine(span[0], span[1], span[2], span[3]);
                case 5:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4]);
                case 6:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5]);
                case 7:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5], span[6]);
                case 8:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5], span[6], span[7]);
                default:
                {
                    var hasher = new HashCode();
                    hasher.AddMany<T>(span);
                    return hasher.ToHashCode();
                }
            }
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="Span{T}"/> according to an <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public static int Combine<T>(scoped Span<T> span, IEqualityComparer<T>? comparer)
        {
            var hasher = new HashCode();
            hasher.AddMany<T>(span, comparer);
            return hasher.ToHashCode();
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        public static int Combine<T>(scoped ReadOnlySpan<T> span)
        {
            switch (span.Length)
            {
                case 0:
                    return _emptyHash;
                case 1:
                    return HashCode.Combine(span[0]);
                case 2:
                    return HashCode.Combine(span[0], span[1]);
                case 3:
                    return HashCode.Combine(span[0], span[1], span[2]);
                case 4:
                    return HashCode.Combine(span[0], span[1], span[2], span[3]);
                case 5:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4]);
                case 6:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5]);
                case 7:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5], span[6]);
                case 8:
                    return HashCode.Combine(span[0], span[1], span[2], span[3], span[4], span[5], span[6], span[7]);
                default:
                {
                    var hasher = new HashCode();
                    hasher.AddMany<T>(span);
                    return hasher.ToHashCode();
                }
            }
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="ReadOnlySpan{T}"/> according to an <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public static int Combine<T>(scoped ReadOnlySpan<T> span, IEqualityComparer<T?>? comparer)
        {
            var hasher = new HashCode();
            hasher.AddMany<T>(span, comparer);
            return hasher.ToHashCode();
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="Array">T[]</see>.
        /// </summary>
        public static int Combine<T>(T[]? array)
        {
            if (array is null)
                return _nullHash;
            switch (array.Length)
            {
                case 0:
                    return _emptyHash;
                case 1:
                    return HashCode.Combine(array[0]);
                case 2:
                    return HashCode.Combine(array[0], array[1]);
                case 3:
                    return HashCode.Combine(array[0], array[1], array[2]);
                case 4:
                    return HashCode.Combine(array[0], array[1], array[2], array[3]);
                case 5:
                    return HashCode.Combine(array[0], array[1], array[2], array[3], array[4]);
                case 6:
                    return HashCode.Combine(array[0], array[1], array[2], array[3], array[4], array[5]);
                case 7:
                    return HashCode.Combine(array[0], array[1], array[2], array[3], array[4], array[5], array[6]);
                case 8:
                    return HashCode.Combine(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
                default:
                {
                    var hasher = new HashCode();
                    hasher.AddMany<T>(array);
                    return hasher.ToHashCode();
                }
            }
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> items in a <see cref="Array">T[]</see> according to an <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public static int Combine<T>(T[]? array, IEqualityComparer<T?>? comparer)
        {
            if (array is null)
                return _nullHash;
            var hasher = new HashCode();
            hasher.AddMany<T>(array, comparer);
            return hasher.ToHashCode();
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> values in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static int Combine<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable is null)
                return _nullHash;
            var hasher = new HashCode();
            hasher.AddMany<T>(enumerable);
            return hasher.ToHashCode();
        }

        /// <summary>
        /// Gets a hashcode generated from all the <typeparamref name="T"/> values in an <see cref="IEnumerable{T}"/> according to an <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public static int Combine<T>(IEnumerable<T>? enumerable, IEqualityComparer<T?>? comparer)
        {
            if (enumerable is null)
                return _nullHash;
            var hasher = new HashCode();
            hasher.AddMany<T>(enumerable, comparer);
            return hasher.ToHashCode();
        }
    }

    extension(ref HashCode hashCode)
    {
#region AddMany
        /// <summary>
        /// Adds the hashcodes of the items in a <see cref="Span{T}"/>
        /// </summary>
        public void AddMany<T>(scoped Span<T> values)
        {
            foreach (var value in values)
            {
                hashCode.Add<T>(value);
            }
        }

        /// <summary>
        /// Adds the hashcodes of the items in a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
        public void AddMany<T>(params ReadOnlySpan<T> values)
        {
            foreach (var value in values)
            {
                hashCode.Add<T>(value);
            }
        }

        /// <summary>
        /// Adds the hashcodes generated by a <paramref name="comparer"/> for the given <paramref name="values"/> to this <see cref="HashCode"/>
        /// </summary>
        public void AddMany<T>(scoped ReadOnlySpan<T> values, IEqualityComparer<T>? comparer)
        {
            foreach (var value in values)
            {
                hashCode.Add<T>(value, comparer);
            }
        }

        /// <summary>
        /// Adds the hashcodes generated for the given <paramref name="values"/> to this <see cref="HashCode"/>
        /// </summary>
        public void AddMany<T>(T[]? values)
        {
            if (values is null)
                return;
            foreach (var value in values)
            {
                hashCode.Add<T>(value);
            }
        }

        /// <summary>
        /// Adds the hashcodes generated by a <paramref name="comparer"/> for the given <paramref name="values"/> to this <see cref="HashCode"/>
        /// </summary>
        public void AddMany<T>(T[]? values, IEqualityComparer<T>? comparer)
        {
            if (values is null)
                return;
            foreach (var value in values)
            {
                hashCode.Add<T>(value, comparer);
            }
        }

        /// <summary>
        /// Adds the hashcodes generated for the given <paramref name="values"/> to this <see cref="HashCode"/>
        /// </summary>
        public void AddMany<T>(IEnumerable<T>? values)
        {
            if (values is null)
                return;
            foreach (var value in values)
            {
                hashCode.Add<T>(value);
            }
        }

        /// <summary>
        /// Adds the hashcodes generated by a <paramref name="comparer"/> for the given <paramref name="values"/> to this <see cref="HashCode"/>
        /// </summary>
        public void AddMany<T>(IEnumerable<T>? values, IEqualityComparer<T>? comparer)
        {
            if (values is null)
                return;
            foreach (var value in values)
            {
                hashCode.Add<T>(value, comparer);
            }
        }
#endregion

#region AddBytes
        /// <summary>
        /// Adds a span of bytes to this <see cref="HashCode"/>.
        /// </summary>
#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public void AddBytes(scoped bytesview bytes)
        {
            // Add four bytes at a time until the input has fewer than four bytes remaining.
            while (bytes.Length >= 4)
            {
                int hash = Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(bytes));
                HashCodeAddInt32Hash(ref hashCode, hash);
                bytes = bytes[4..];
            }

            // Add the remaining bytes
            foreach (byte u8 in bytes)
            {
                HashCodeAddInt32Hash(ref hashCode, (int)u8);
            }
        }
#endregion /AddBytes
    }
}