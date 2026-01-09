namespace ScrubJay.Memory.Splitting;

[PublicAPI]
[Flags]
public enum SplitOptions
{
    None = 0,
    IgnoreEmpty = 1 << 0,
}