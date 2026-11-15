namespace ScrubJay.Testing.Naughty;

public sealed class ThrowingToString
{
    public override string ToString() => throw new InvalidOperationException("Naughty ToString.");
}