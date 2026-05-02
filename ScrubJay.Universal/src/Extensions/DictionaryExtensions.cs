namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension<D, K, V>(D? dictionary)
        where D : IDictionary<K, V>
    {
        public void AddMany(params ReadOnlySpan<KeyValuePair<K, V>> entries)
        {
            if (dictionary is not null)
            {
                foreach (var entry in entries)
                {
                    dictionary.Add(entry.Key, entry.Value);
                }
            }
        }

        public void AddMany(IEnumerable<KeyValuePair<K, V>>? entries)
        {
            if (dictionary is not null && entries is not null)
            {
                foreach (var entry in entries)
                {
                    dictionary.Add(entry.Key, entry.Value);
                }
            }
        }
    }
}