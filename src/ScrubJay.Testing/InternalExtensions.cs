namespace ScrubJay.Testing;

internal static class InternalExtensions
{
    extension(HashSet<char> characters)
    {
        public void AddRange(scoped text text)
        {
            foreach (char ch in text)
            {
                characters.Add(ch);
            }
        }
    }
}