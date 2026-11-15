namespace ScrubJay.Testing.Naughty;

public sealed class ThrowingDispose : IDisposable
{
    public void Dispose() => throw new InvalidOperationException("Naughty Dispose.");
}