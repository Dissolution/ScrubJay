namespace ScrubJay.Testing.OldThings;

#pragma warning disable CA1066, CA1815, CA2231
public struct StructThing
#pragma warning restore CA1066, CA1815, CA2231
{
    private readonly Guid _id;
    private string? _name;

    public Guid Id => _id;

    public string? Name
    {
        get => _name;
        set => _name = value;
    }

    public StructThing(string? name = null)
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