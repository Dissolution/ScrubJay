namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class ListExtensions
{
    extension<T>(IList<T>? list)
    {
        public bool IsNullOrEmpty() => list is null || list.Count == 0;

        public void AddMany(params ReadOnlySpan<T> items)
        {
            if (list is not null)
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        public void AddMany(IEnumerable<T>? items)
        {
            if (list is not null && items is not null)
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }
    }
}