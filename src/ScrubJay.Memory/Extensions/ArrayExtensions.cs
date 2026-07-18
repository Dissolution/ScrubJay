namespace ScrubJay.Memory.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[] array)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void* UnsafeAsVoidPointer() => Unsafe.AsPointer<T>(ref MemoryMarshal.GetArrayDataReference(array));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void* UnsafeAsVoidPointer(int offset)
        {
            return Unsafe.AsPointer<T>(
                ref Unsafe.Add<T>(
                    ref MemoryMarshal.GetArrayDataReference(array),
                    offset));
        }
    }

    extension<T>(T[]? array)
    {
        public bool TryAsSpan(Range range, out Span<T> span)
        {
            if (array is null)
            {
                span = default;
                return true;
            }
            
            var (offset, length) = range.GetOffsetAndLength(array.Length);
            if ((uint)offset + (uint)length <= array.Length)
            {
                unsafe
                {
                    span = new Span<T>(array.UnsafeAsVoidPointer(offset), length);
                }
                return true;
            }

            span = default;
            return false;
        }

        public bool TryAsSpan(int start, int length, out Span<T> span)
        {
            if (array is null)
            {
                span = default;
                return true;
            }

            if ((uint)start + (uint)length <= array.Length)
            {
                unsafe
                {
                    span = new Span<T>(array.UnsafeAsVoidPointer(start), length);
                }
                return true;
            }

            span = default;
            return false;
        }

        public bool TrySelfCopy(int sourceIndex, int destIndex, int itemCount)
        {
            if (array is null) return false;
            return array.TryAsSpan(sourceIndex, itemCount, out var source) &&
                array.TryAsSpan(destIndex, itemCount, out var destination) &&
                source.TryCopyTo(destination);
        }

        public bool TrySelfCopy(Range source, int destIndex)
        {
            if (array is null) return false;
            var (sourceIndex, length) = source.GetOffsetAndLength(array.Length);
            return array.TrySelfCopy(sourceIndex, destIndex, length);
        }

        public bool TrySelfCopy(int sourceIndex, Range dest)
        {
            if (array is null) return false;
            var (destIndex, length) = dest.GetOffsetAndLength(array.Length);
            return array.TrySelfCopy(sourceIndex, destIndex, length);
        }
    }
}