namespace ScrubJay;

public static class Prelude
{
    public static string Build(InterpolatedTextBuilder interpolatedText)
    {
        return interpolatedText.ToStringAndClear();
    }
}