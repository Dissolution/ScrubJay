namespace ScrubJay.Extensions;

[PublicAPI]
public static class HashSetExtensions
{
    extension<T>(HashSet<T> set)
    {
        public void AddMany(params ReadOnlySpan<T> items)
        {
            foreach (var item in items)
            {
                _ = set.Add(item);
            }
        }

        public void AddMany(IEnumerable<T>? items)
        {
            if (items is null)
                return;
            foreach (var item in items)
            {
                _ = set.Add(item);
            }
        }
    }
}