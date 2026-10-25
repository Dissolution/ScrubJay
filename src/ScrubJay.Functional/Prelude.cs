namespace ScrubJay.Functional;

[PublicAPI]
public static class Prelude
{
    public static Option.None None() => default;
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    public static Result<T, E> Ok<T, E>(T value) => Result<T, E>.Ok(value);

    public static Result<T, E> Error<T, E>(E error) => Result<T, E>.Error(error);
}