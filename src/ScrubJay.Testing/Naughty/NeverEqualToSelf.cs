namespace ScrubJay.Testing.Naughty;

public sealed class NeverEqualToSelf
{
    public override bool Equals(object? obj) => false;

    public override int GetHashCode() => 0;
}