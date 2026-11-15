namespace ScrubJay.Testing.Naughty;

public sealed class ThrowingProperty
{
    public string Name => "has a throwing property";

    public int Boom => throw new InvalidOperationException("Naughty property.");
}