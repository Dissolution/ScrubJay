namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyEnumerator : IEnumerator
{
    public static readonly EmptyEnumerator Default;

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
}

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
#if NET9_0_OR_GREATER
where T : allows ref struct
#endif
{
    public static readonly EmptyEnumerator<T> Default;

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
}