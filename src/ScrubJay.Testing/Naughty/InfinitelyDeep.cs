namespace ScrubJay.Testing.Naughty;

public sealed class InfinitelyDeep
{
    public InfinitelyDeep Child => new();
}