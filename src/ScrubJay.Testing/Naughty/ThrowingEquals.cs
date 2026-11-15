namespace ScrubJay.Testing.Naughty;

public sealed class ThrowingEquals
{
    public override bool Equals(object? obj) => throw new InvalidOperationException("Naughty Equals.");

    public override int GetHashCode() => 0;
}