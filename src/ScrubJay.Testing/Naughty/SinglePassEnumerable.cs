namespace ScrubJay.Testing.Naughty;

public sealed class SinglePassEnumerable : IEnumerable<int>, IEnumerable
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

    public SinglePassEnumerator GetEnumerator() => new SinglePassEnumerator();
}