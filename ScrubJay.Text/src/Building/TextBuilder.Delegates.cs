namespace ScrubJay.Text.Building;

public ref partial struct TextBuilder
{
    public delegate void BuildWithInterpolatedText(TextBuilder builder, ref InterpolatedTextBuilder interpolatedTextBuilder);
}