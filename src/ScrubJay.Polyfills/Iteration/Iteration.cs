namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
public static class Iteration
{
    public static EmptyIteration Empty() => default;

    public static EmptyIteration<T> Empty<T>() => default;

    public static SingleIteration<T> Single<T>(T value) => new SingleIteration<T>(value);
}