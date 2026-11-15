namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[] array)
    {
        public Option<T> TryGetAt(int index)
        {
            if ((uint)index < (uint)array.Length)
                return array[index];
            return default;
        }
        
        public Option<T> TryGetAt(Index index)
        {
            int offset = index.GetOffset(array.Length);
            if ((uint)offset < (uint)array.Length)
                return array[offset];
            return default;
        }

        public bool TrySetAt(int index, T item)
        {
            if ((uint)index < (uint)array.Length)
            {
                array[index] = item;
                return true;
            }
            return false;
        }
        
        public bool TrySetAt(Index index, T item)
        {
            int offset = index.GetOffset(array.Length);
            if ((uint)offset < (uint)array.Length)
            {
                array[offset] = item;
                return true;
            }
            return false;
        }
    }
}