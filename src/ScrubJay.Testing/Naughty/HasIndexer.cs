namespace ScrubJay.Testing.Naughty;

public sealed class HasIndexer
{
    public string Name => "has an indexer";

    public int this[int index] => index;
}