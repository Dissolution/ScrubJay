namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class ListExtensions
{
    extension<L, T>(L? list)
        where L : IList<T>
    {
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