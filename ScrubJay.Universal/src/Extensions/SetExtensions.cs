namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class SetExtensions
{
    extension<T>(ISet<T>? set)
    {
        public bool IsNullOrEmpty() => set is null || set.Count == 0;

        public void AddMany(params ReadOnlySpan<T> items)
        {
            if (set is not null)
            {
                foreach (var item in items)
                {
                    _ = set.Add(item);
                }
            }
        }

        public void AddMany(IEnumerable<T>? items)
        {
            if (set is not null && items is not null)
            {
                foreach (var item in items)
                {
                    _ = set.Add(item);
                }
            }
        }
    }
}