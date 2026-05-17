namespace ScrubJay.Text.Building;

[PublicAPI]
public readonly record struct AlignmentOptions
{
    public static implicit operator AlignmentOptions(Alignment alignment) => new(alignment: alignment);

    public static readonly AlignmentOptions Default = new();

    public readonly char PaddingChar;
    public readonly Alignment Alignment;
    public readonly bool TruncateToWidth;
    public readonly char? TruncateChar;

    public AlignmentOptions(
        char paddingChar = ' ',
        Alignment alignment = Alignment.Right,
        bool truncateToWidth = true,
        char? truncateChar = '…')
    {
        PaddingChar = paddingChar;
        Alignment = alignment;
        TruncateToWidth = truncateToWidth;
        TruncateChar = truncateChar;
    }

    public void Deconstruct(out char paddingChar, out Alignment alignment, out bool truncateToWidth, out char? truncateChar)
    {
        paddingChar = PaddingChar;
        alignment = Alignment;
        truncateToWidth = TruncateToWidth;
        truncateChar = TruncateChar;
    }
}