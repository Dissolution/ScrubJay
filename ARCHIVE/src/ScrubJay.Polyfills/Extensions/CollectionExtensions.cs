namespace ScrubJay.Polyfills;

[PublicAPI]
public static class CollectionExtensions
{
    extension<T>(ICollection<T>? collection)
    {
        public void ForEach(Action<T>? perItem)
        {
            if (collection is null || perItem is null) return;
            foreach (T item in collection)
            {
                perItem(item);
            }
        }

        public void AddMany(params ReadOnlySpan<T> items)
        {
            if (collection is null) return;

            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        public void AddMany(IEnumerable<T>? items)
        {
            if (collection is null || items is null) return;

            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
    }
}