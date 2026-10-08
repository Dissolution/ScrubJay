namespace ScrubJay.Testing.Things;

public ref struct RefStructThing
{
    private readonly Guid _id;
    private string? _name;

    public Guid Id => _id;

    public string? Name
    {
        get => _name;
        set => _name = value;
    }

    public RefStructThing(string? name = null)
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

    public override string ToString() => $"{typeof(RefStructThing)} #{Id:D} \"{_name}\"";
}