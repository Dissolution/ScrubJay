#if NET9_0_OR_GREATER

namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public ref struct RSSingleEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
where T : allows ref struct
{
    public static implicit operator RSSingleEnumerator<T>(EmptyEnumerator _) => new();
    public static implicit operator RSSingleEnumerator<T>(EmptyEnumerator<T> _) => new();
    
    private readonly T _value;
    private bool _canYield;

    object? IEnumerator.Current => Any.BoxOrNull(_value);
    
    public T Current => _value;
    
    public RSSingleEnumerator(T value)
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
    
    void IDisposable.Dispose()
    {
        // do nothing
    }
}

#endif