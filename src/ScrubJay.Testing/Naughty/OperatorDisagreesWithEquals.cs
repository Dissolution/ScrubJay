namespace ScrubJay.Testing.Naughty;

public sealed class OperatorDisagreesWithEquals
{
    public static bool operator ==(OperatorDisagreesWithEquals? left, OperatorDisagreesWithEquals? right) => true;

    public static bool operator !=(OperatorDisagreesWithEquals? left, OperatorDisagreesWithEquals? right) => false;

    public override bool Equals(object? obj) => ReferenceEquals(this, obj);

    public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);
}