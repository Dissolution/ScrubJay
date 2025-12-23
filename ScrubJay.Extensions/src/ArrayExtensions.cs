namespace ScrubJay.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[] array)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(scoped ReadOnlySpan<T> source)
        {
            source.CopyTo(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(scoped Span<T> source)
        {
            source.CopyTo(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(T[]? source)
        {
            source.CopyTo(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(ICollection<T>? collection)
        {
            if (collection is not null)
            {
                collection.CopyTo(array, 0);
            }
        }
    }
}