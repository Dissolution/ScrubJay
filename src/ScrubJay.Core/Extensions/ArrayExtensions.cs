namespace ScrubJay.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
//    extension<T>(T[] array)
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        internal unsafe void* UnsafeAsVoidPointer() => Unsafe.AsPointer<T>(ref MemoryMarshal.GetArrayDataReference(array));
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        internal unsafe void* UnsafeAsVoidPointer(int offset)
//        {
//            return Unsafe.AsPointer<T>(
//                ref Unsafe.Add<T>(
//                    ref MemoryMarshal.GetArrayDataReference(array),
//                    offset));
//        }
//    }

    extension<T>(T[]? array)
    {
        public Span<T> Slice(int start) => array.AsSpan(start);
        public Span<T> Slice(int start, int length) => array.AsSpan(start, length);

        public bool TrySlice(int start, out Span<T> slice)
        {
            if (array is not null && (uint)start < (uint)array.Length)
            {
                slice = array.AsSpan(start);
                return true;
            }

            slice = default;
            return false;
        }

        public bool TrySlice(Index start, out Span<T> slice)
        {
            if (array is not null && start.TryGetOffset(array.Length, out var s))
            {
                slice = array.AsSpan(s);
                return true;
            }

            slice = default;
            return false;
        }

        public bool TrySlice(int start, int length, out Span<T> slice)
        {
            if (array is not null && ((uint)start + (uint)length) <= (uint)array.Length)
            {
                slice = array.AsSpan(start, length);
                return true;
            }

            slice = default;
            return false;
        }

        public bool TrySlice(Range range, out Span<T> slice)
        {
            if (array is not null && range.TryGetOffsetAndLength(array.Length, out int offset, out int length))
            {
                slice = array.AsSpan(offset, length);
                return true;
            }

            slice = default;
            return false;
        }

        public bool TrySelfCopy(int sourceIndex, int destIndex, int itemCount)
        {
            return array.TrySlice(sourceIndex, itemCount, out var source)
                && array.TrySlice(destIndex, itemCount, out var destination)
                && source.TryCopyTo(destination);
        }

        public bool TrySelfCopy(Range sourceRange, int destIndex)
        {
            return array.TrySlice(sourceRange, out var source)
                && array.TrySlice(destIndex, out var destination)
                && source.TryCopyTo(destination);
        }

        public bool TrySelfCopy(int sourceIndex, Range dest)
        {
            return array.TrySlice(dest, out var destination)
                && array.TrySlice(sourceIndex, destination.Length, out var source)
                && source.TryCopyTo(destination);
        }
    }
}