namespace ScrubJay.Text.Comparison;

[PublicAPI]
public interface ITextEqualityComparer :
#if NET9_0_OR_GREATER
    IEqualityComparer<text>,
#endif
    IEqualityComparer<char>,
    IEqualityComparer<string>,
    IEqualityComparer<char[]>;