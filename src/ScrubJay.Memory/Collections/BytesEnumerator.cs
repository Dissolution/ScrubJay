namespace ScrubJay.Memory.Collections;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public ref struct BytesEnumerator : IEnumerator<byte>, IEnumerator, IDisposable
{
    private bytes _bytes;
    private int _index;

    object IEnumerator.Current => Current;

    byte IEnumerator<byte>.Current => Current;
    
    public ref byte Current => ref _bytes[_index];

    public BytesEnumerator(bytes bytes)
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