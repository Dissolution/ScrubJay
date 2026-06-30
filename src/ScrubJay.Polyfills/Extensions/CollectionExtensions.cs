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
    }
}