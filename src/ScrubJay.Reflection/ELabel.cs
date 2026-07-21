using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection.IL;

namespace ScrubJay.Reflection;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public struct ELabel :
#if NET7_0_OR_GREATER
    IEqualityOperators<ELabel, ELabel, bool>,
#endif
    IEquatable<ELabel>,
    IEquatable<Label>
{
    public static bool operator ==(ELabel left, ELabel right) => left.Equals(right);
    public static bool operator !=(ELabel left, ELabel right) => !left.Equals(right);
    
    public readonly int Id;
    public string? Name;
    public ILOffset Offset;

    public ELabel(int id, string? name = null)
    {
        this.Id = id;
        this.Name = name;
    }

    public ELabel(int id, ILOffset offset, string? name = null)
    {
        this.Id = id;
        this.Offset = offset;
        this.Name = name;
    }

    public bool IsShortForm => Id <= 127;
    
    public bool Equals(ELabel other)
    {
        return Id == other.Id;
    }
    
    public bool Equals(Label other)
    {
        return Id == other.GetHashCode(); // same as Id
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ELabel eLabel)
            return Equals(eLabel);
        if (obj is Label label)
            return Equals(label);
        return false;
    }

    public override int GetHashCode() => Id;

    public override string ToString()
    {
        using InterpolatedText builder = new(4, 3);
        builder.Write(Offset);
        builder.Write(": Label #");
        builder.Write(Id);
        if (Name.IsNotEmpty())
        {
            builder.Write($" \"{Name}\"");
        }
        return builder.ToString();
    }
}