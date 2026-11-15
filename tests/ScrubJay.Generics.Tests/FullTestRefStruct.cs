using ScrubJay.Generics.Extensions;

namespace ScrubJay.Generics.Tests;

[UsedImplicitly]
internal ref struct FullTestRefStruct
{
    public readonly int Id;
    public string? Name;

    public FullTestRefStruct()
    {
        var guid = Guid.NewGuid();
        Id = guid.GetHashCode();
        Name = BitConverter.ToString(guid.ToByteArray());
    }

    public FullTestRefStruct(int id, string? name = null)
    {
        this.Id = id;
        this.Name = name;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => false;

    public override int GetHashCode() => Id;

    public override string ToString() => $"{typeof(FullTestRefStruct).Alias}-id:{Id}-name:{Name}";
}