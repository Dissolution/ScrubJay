namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T>? enumerable)
    {
        public void Consume(Action<T> perItem)
        {
            if (enumerable is null)
                return;
            foreach (var item in enumerable)
            {
                perItem(item);
            }
        }
    }
}