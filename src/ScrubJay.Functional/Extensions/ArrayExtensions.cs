namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[]? array)
    {
        public Result<T, Exception> TryGet(Index index)
        {
            if (array is null)
                return new ArgumentNullException(nameof(array));
            int offset = index.GetOffset(array.Length);
            if ((uint)offset >= (uint)array.Length)
                return new ArgumentOutOfRangeException(nameof(index), index, $"Index was not in [{array.Length}]");
            return array[offset];
        }

        public Result<Unit,Exception> TrySet(Index index, T item)
        {
            if (array is null)
                return new ArgumentNullException(nameof(array));
            int offset = index.GetOffset(array.Length);
            if ((uint)offset >= (uint)array.Length)
                return new ArgumentOutOfRangeException(nameof(index), index, $"Index was not in [{array.Length}]");
            array[offset] = item;
            return default(Unit);
        }
    }
}