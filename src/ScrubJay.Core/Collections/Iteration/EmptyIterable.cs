#pragma warning disable CA1010

using ScrubJay.Errors;

namespace ScrubJay.Collections.Iteration;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyIterable : IEnumerable, IEnumerator
{
    object IEnumerator.Current
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

    public EmptyIterable GetEnumerator => this;
}

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
public readonly struct EmptyIterable<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    object IEnumerator.Current
    {
        [DoesNotReturn]
        get => throw Ex.Enumeration.CannotHappen();
    }

    T IEnumerator<T>.Current
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

    public EmptyIterable<T> GetEnumerator() => this;
}