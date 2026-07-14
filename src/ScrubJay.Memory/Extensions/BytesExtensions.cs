using ScrubJay.Functional;
using ScrubJay.Memory.Collections;
using ScrubJay.Polyfills;

namespace ScrubJay.Memory.Extensions;

[PublicAPI]
public static class BytesExtensions
{
    extension<B>(ref B bytes)
        where B : unmanaged, IBytes
    {
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Unsafe.SizeOf<B>();
            }
        }

        public byte GetAt(int index) => bytes.RefItem<B>(index);

        public Option<byte> TryGetAt(int index)
        {
            unsafe
            {
                if ((uint)index >= (uint)sizeof(B))
                    return default;
                ref B refBytes = ref Unsafe.AsRef<B>(in bytes);
                ref byte refByte = ref Unsafe.As<B, byte>(ref refBytes);
                return Unsafe.Add(ref refByte, index);
            }
        }
        
        public ref byte RefItem(int index)
        {
            unsafe
            {
                if ((uint)index >= (uint)sizeof(B))
                    throw new ArgumentOutOfRangeException(nameof(index), index, $"Index must be in [0..{sizeof(B)})");
                ref byte b = ref Unsafe.As<B, byte>(ref bytes);
                return ref Unsafe.Add(ref b, index);
            }
        }
        
        public void SetAt(int index, byte u8) => bytes.RefItem(index) = u8;

        public Span<byte> AsSpan()
        {
            unsafe
            {
                void* ptr = Unsafe.RefAsVoidPtr(ref bytes);
                return new Span<byte>(ptr, sizeof(B));
            }
        }

        
        public ReadOnlySpan<byte> AsReadOnlySpan()
        {
            unsafe
            {
                void* ptr = Unsafe.RefAsVoidPtr(ref bytes);
                return new ReadOnlySpan<byte>(ptr, sizeof(B));
            }
        }

        public bool Equals(ref B other)
        {
            var bytesSpan = BytesExtensions.AsReadOnlySpan<B>(ref bytes);
            var otherSpan = BytesExtensions.AsReadOnlySpan<B>(ref other);
            return bytesSpan.SequenceEqual(otherSpan);
        }

        public bool Equals(scoped ReadOnlySpan<byte> other)
        {
            var bytesSpan = BytesExtensions.AsReadOnlySpan<B>(ref bytes);
            return bytesSpan.SequenceEqual(other);
        }
    }

    extension(bytes bytes)
    {
        
    }
}