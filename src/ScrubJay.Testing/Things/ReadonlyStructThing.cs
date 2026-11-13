// ReSharper disable UnusedType.Global
namespace ScrubJay.Testing.Things;

#pragma warning disable CA1066
#pragma warning disable CA2231
#pragma warning disable CA1815
public readonly struct ReadonlyStructThing
#pragma warning restore CA1815
#pragma warning restore CA2231
#pragma warning restore CA1066
{
    private readonly Guid _id;
    private readonly string? _name;

    public Guid Id => _id;

    public string? Name
    {
        get => _name;
    }

    public ReadonlyStructThing(string? name = null)
    {
        _id = Guid.NewGuid();
        _name = name;
    }

    public void Deconstruct(out Guid id, out string? name)
    {
        id = _id;
        name = _name;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => false;

    public override int GetHashCode() => _id.GetHashCode();

    public override string ToString() => $"{GetType()} #{Id:D} \"{_name}\"";
}