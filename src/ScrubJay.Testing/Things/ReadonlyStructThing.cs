namespace ScrubJay.Testing.Things;

public readonly struct ReadonlyStructThing
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