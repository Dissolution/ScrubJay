namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
public static class Enumerator
{
    public static EmptyEnumerator Empty() => EmptyEnumerator.Default;

    public static EmptyEnumerator<T> Empty<T>() => EmptyEnumerator<T>.Default;

    public static SingleEnumerator<T> Single<T>(T value) => new SingleEnumerator<T>(value);
}