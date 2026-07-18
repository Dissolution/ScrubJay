namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyIteration : IIterable<EmptyIteration>, IIterator
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

    public IEnumerator GetEnumerator() => this;

    public EmptyIteration GetIterator() => this;

    void IDisposable.Dispose()
    {
        // do nothing
    }
}

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyIteration<T> : IIterable<EmptyIteration<T>, T>, IIterator<T>
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

    public EmptyIteration<T> GetIterator() => this;
}