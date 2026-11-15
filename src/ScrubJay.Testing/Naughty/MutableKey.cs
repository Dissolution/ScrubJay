namespace ScrubJay.Testing.Naughty;

public sealed class MutableKey
{
    public int Id { get; set; }

    public override bool Equals(object? obj) => obj is MutableKey other && other.Id == Id;

    public override int GetHashCode() => Id;
}