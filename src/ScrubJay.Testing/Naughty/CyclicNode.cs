namespace ScrubJay.Testing.Naughty;

public sealed class CyclicNode
{
    public string Name { get; set; } = "node";

    public CyclicNode? Next { get; set; }
}