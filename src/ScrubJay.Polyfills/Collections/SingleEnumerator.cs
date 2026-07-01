namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct SingleEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
{
    private readonly T _value;
    private bool _canYield;

    object? IEnumerator.Current => _value;
    public T Current => _value;
    
    public SingleEnumerator(T value)
    {
        _value = value;
        _canYield = false;
    }

    public bool MoveNext()
    {
        if (_canYield)
        {
            _canYield = false;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _canYield = true;
    }
    
    [HandlesResourceDisposal]
    void IDisposable.Dispose()
    {
        // do nothing
    }
}