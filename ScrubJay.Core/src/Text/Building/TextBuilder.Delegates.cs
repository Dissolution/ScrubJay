namespace ScrubJay.Text.Building;

partial class TextBuilder
{
    public delegate void BuildWithInterpolatedText(TextBuilder builder, ref InterpolatedTextBuilder interpolatedTextBuilder);
}