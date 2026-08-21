#pragma warning disable CA1716

namespace ScrubJay.Functional;

/// <summary>
/// A static utility class for constructing <see cref="Option{T}"/> instances
/// </summary>
[PublicAPI]
public static class Option
{
    public static IMPL.None None() => default;
    public static Option<T> None<T>() => default;
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <see langword="null"/>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> NotNull<T>(T? value)
    {
        if (value is not null)
            return Option<T>.Some(value);
        return default;
    }

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <see langword="null"/>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> NotNull<T>(Nullable<T> value)
        where T : struct
    {
        if (value.HasValue)
        {
            return Option<T>.Some(value.GetValueOrDefault());
        }

        return default;
    }
}