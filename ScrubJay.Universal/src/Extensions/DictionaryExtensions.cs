namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension<K, V>(IDictionary<K, V>? dictionary)
        where K : notnull
    {
        public bool IsNullOrEmpty() => dictionary is null || dictionary.Count == 0;

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