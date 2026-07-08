namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct ArrayEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
{
    private readonly T[] _array;
    private int _position;

    object? IEnumerator.Current => Current;

    T IEnumerator<T>.Current => Current;

    public ref T Current
    {
        get
        {
            if (_position < 0)
                throw Ex.Enumeration.NotStarted();
            if (_position >= _array.Length)
                throw Ex.Enumeration.Finished();
            return ref _array[_position];
        }
    }

    public ArrayEnumerator(T[] array)
    {
        _array = array;
        _position = -1;
    }

    public bool MoveNext()
    {
        int newPos = _position + 1;
        if (newPos >= _array.Length)
            return false;

        _position = newPos;
        return true;
    }

    public void Reset()
    {
        _position = -1;
    }

    void IDisposable.Dispose()
    {
        // do nothing
    }
}