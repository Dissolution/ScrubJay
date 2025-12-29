namespace ScrubJay.Interpolated.Tests;

public sealed record class TypeExpected(Type? Type, string Name)
{
    public override string ToString() => Name;
}

public sealed class TypeExpectedData : TheoryData<TypeExpected>
{
    public TypeExpectedData() : base()
    {
        
    }
    
    public TypeExpectedData(IEnumerable<TypeExpected> expected) : base(expected)
    {
        
    }
    
    public void Add(Type? type, string name) => base.Add(new (new(type, name)));
    
    public void Add((Type? Type, string Name) tuple) => base.Add(new(new(tuple.Type, tuple.Name)));
}