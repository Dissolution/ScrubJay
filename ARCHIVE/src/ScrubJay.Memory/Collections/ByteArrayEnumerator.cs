namespace ScrubJay.Memory.Collections;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
[MustDisposeResource(false)]
public struct ByteArrayEnumerator : IEnumerator<byte>, IEnumerator, IDisposable
{
    private readonly byte[] _bytes;
    private int _index;

    object IEnumerator.Current => Current;

    public byte Current => _bytes[_index];

    public ByteArrayEnumerator(params byte[]? bytes)
    {
        _bytes = bytes ?? [];
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