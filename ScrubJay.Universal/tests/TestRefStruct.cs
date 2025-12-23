using System.Diagnostics.CodeAnalysis;

namespace ScrubJay.Universal.Tests;

public readonly ref struct TestRefStruct : IEquatable<TestRefStruct>, IComparable<TestRefStruct>
{
    public readonly int Id;
    
    public readonly string? Name;

    public TestRefStruct(int id, string? name)
    {
        Id = id;
        Name = name;
    }

    public int CompareTo(TestRefStruct other)
    {
        return this.Id.CompareTo(other.Id);
    }
    
    public bool Equals(TestRefStruct other)
    {
        return this.Id.Equals(other.Id);
    }
    
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is int id)
            return Id == id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public override string ToString()
    {
        return $"#{Id}: {Name}";
    }
}