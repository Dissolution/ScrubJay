namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyEnumera : IEnumerable, IEnumerator
{
    public object Current
    {
        [DoesNotReturn]
        get => throw Ex.Enumeration.CannotHappen();
    }

    public bool MoveNext() => false;

    public void Reset()
    {
        // do nothing
    }

    IEnumerator IEnumerable.GetEnumerator() => this;

    public EmptyEnumera GetEnumerator => this;
}

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyEnumera<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    object IEnumerator.Current
    {
        [DoesNotReturn]
        get => throw Ex.Enumeration.CannotHappen();
    }

    public T Current
    {
        [DoesNotReturn]
        get => throw Ex.Enumeration.CannotHappen();
    }

    public bool MoveNext() => false;

    public void Reset()
    {
        // do nothing
    }

    void IDisposable.Dispose()
    {
        // do nothing
    }

    IEnumerator IEnumerable.GetEnumerator() => this;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;

    public EmptyEnumera<T> GetEnumerator() => this;
}