namespace ScrubJay;

public static class Prelude
{
    public static string Build(ref InterpolatedTextBuilder interpolatedText)
    {
        return interpolatedText.ToStringAndDispose();
    }
}