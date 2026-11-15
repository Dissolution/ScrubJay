namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class ListExtensions
{
    extension<T>(IList<T> list)
    {
        public Option<T> TryGetAt(int index)
        {
            if ((uint)index < (uint)list.Count)
                return list[index];
            return default;
        }

        public Option<T> TryGetAt(Index index)
        {
            int offset = index.GetOffset(list.Count);
            if ((uint)offset < (uint)list.Count)
                return list[offset];
            return default;
        }

        public bool TrySetAt(int index, T item)
        {
            if ((uint)index < (uint)list.Count)
            {
                list[index] = item;
                return true;
            }
            return false;
        }

        public bool TrySetAt(Index index, T item)
        {
            int offset = index.GetOffset(list.Count);
            if ((uint)offset < (uint)list.Count)
            {
                list[offset] = item;
                return true;
            }
            return false;
        }
    }
}