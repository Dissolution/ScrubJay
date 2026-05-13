namespace ScrubJay.Text.Utilities;

[PublicAPI]
public static class TextUtilities
{
    extension(text)
    {
        public static bool StartsWith(scoped text text, scoped text subtext)
        {
            return text.StartsWith(subtext, StringComparison.Ordinal);
        }
    }
}