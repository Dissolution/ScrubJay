namespace ScrubJay.Testing.Naughty;

public sealed class ThrowingGetHashCode
{
    public override bool Equals(object? obj) => ReferenceEquals(this, obj);

    public override int GetHashCode() => throw new InvalidOperationException("Naughty GetHashCode.");
}