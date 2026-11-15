namespace ScrubJay.Testing.Naughty;

/// <summary>
/// One test value, the category it belongs to, and the reason it is on the list.
/// </summary>
/// <remarks>
/// A plain class rather than a record on purpose: a record's generated <c>ToString</c> and <c>Equals</c>
/// would call into <see cref="Value"/>, and several values deliberately throw from those members.
/// </remarks>
public sealed class NaughtyObject
{
    public NaughtyObject(string category, string name, string why, object? value)
    {
        Category = category;
        Name = name;
        Why = why;
        Value = value;
    }

    public string Category { get; }

    public string Name { get; }

    public string Why { get; }

    public object? Value { get; }

    public override string ToString() => $"{Category}: {Name}";
}