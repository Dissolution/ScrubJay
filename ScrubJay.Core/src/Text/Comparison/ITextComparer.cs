namespace ScrubJay.Text.Comparison;

[PublicAPI]
public interface ITextComparer :
#if NET9_0_OR_GREATER
    IComparer<text>,
#endif
    IComparer<char>,
    IComparer<string>,
    IComparer<char[]>;
