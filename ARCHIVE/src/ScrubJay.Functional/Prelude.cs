namespace ScrubJay.Functional;

[PublicAPI]
public static class Prelude
{
    public static Unit Unit() => default;

    public static Impl.None None() => default;

    public static Option<T> None<T>() => default;

    public static Option<T> Some<T>(T value) => new Option<T>(value);

    public static Impl.Ok<T> Ok<T>(T value) => new(value);

    public static Result<T, E> Ok<T, E>(T value) => new(value);

    public static Impl.Error<E> Error<E>(E error) => new(error);

    public static Result<T, E> Error<T, E>(E error) => new(error);
}