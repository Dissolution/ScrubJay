namespace ScrubJay.Testing.Naughty;

public sealed class SinglePassEnumerator : IEnumerator<int>, IEnumerator, IDisposable
{
    private int _index;

    object? IEnumerator.Current => _index;

    public int Current => _index;

    public SinglePassEnumerator()
    {
        _index = -1;
    }

    public bool MoveNext()
    {
        _index++;
        return _index < 3;
    }

    public void Reset()
    {
        throw new InvalidOperationException();
    }

    public void Dispose()
    {
        // do nothing
    }
}