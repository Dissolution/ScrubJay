namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class SetExtensions
{
    extension<S, T>(S? set)
        where S : ISet<T>
    {
        public void AddMany(params ReadOnlySpan<T> items)
        {
            if (set is not null)
            {
                foreach (var item in items)
                {
                    set.Add(item);
                }
            }
        }
        
        public void AddMany(IEnumerable<T>? items)
        {
            if (set is not null && items is not null)
            {
                foreach (var item in items)
                {
                    set.Add(item);
                }
            }
        }
    }
}