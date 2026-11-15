namespace ScrubJay.Testing.Naughty;

public readonly struct Meters
{
    public Meters(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static implicit operator double(Meters meters) => meters.Value;

    public static implicit operator Meters(double value) => new(value);
}