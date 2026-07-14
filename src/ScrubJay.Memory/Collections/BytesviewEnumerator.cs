namespace ScrubJay.Memory.Collections;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public ref struct BytesviewEnumerator : IEnumerator<byte>, IEnumerator, IDisposable
{
    private readonly bytesview _bytes;
    private int _index;

    object IEnumerator.Current => Current;

    byte IEnumerator<byte>.Current => Current;
    
    public byte Current => _bytes[_index];

    public BytesviewEnumerator(bytesview bytes)
    {
        _bytes = bytes;
    }

    public bool MoveNext()
    {
        int nextIndex = _index + 1;
        if (nextIndex >= _bytes.Length)
            return false;

        _index = nextIndex;
        return true;
    }

    public void Reset()
    {
        _index = -1;
    }

    void IDisposable.Dispose()
    {
        // nothing
    }
}