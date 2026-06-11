namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class CollectionExtensions
{
    extension<T>(ICollection<T>? collection)
    {
        public bool IsNullOrEmpty() => collection is null || collection.Count == 0;
        
        public void AddMany(params ReadOnlySpan<T> items)
        {
            if (collection is not null)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }

        public void AddMany(IEnumerable<T>? items)
        {
            if (collection is not null && items is not null)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }
    }
}