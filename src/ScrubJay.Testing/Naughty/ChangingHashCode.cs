namespace ScrubJay.Testing.Naughty;

public sealed class ChangingHashCode
{
    private int _calls;

    public override bool Equals(object? obj) => ReferenceEquals(this, obj);

    public override int GetHashCode() => ++_calls;
}