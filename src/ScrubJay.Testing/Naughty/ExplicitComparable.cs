namespace ScrubJay.Testing.Naughty;

public sealed class ExplicitComparable : IComparable
{
    int IComparable.CompareTo(object? obj) => 0;
}