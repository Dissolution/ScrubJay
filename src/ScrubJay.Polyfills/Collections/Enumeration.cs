namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
public static class Enumeration
{
    public static EmptyEnumera Empty() => default;

    public static EmptyEnumera<T> Empty<T>() => default;

    public static SingleEnumera<T> Single<T>(T value) => new SingleEnumera<T>(value);
}