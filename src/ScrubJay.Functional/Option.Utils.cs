using ScrubJay.Polyfills;
#pragma warning disable CA1716

namespace ScrubJay.Functional;

/// <summary>
/// A static utility class for constructing <see cref="Option{T}"/> instances
/// </summary>
[PublicAPI]
public static class Option
{
#region Constructors
    /// <summary>
    /// Gets a <see cref="IMPL.None"/> that implicitly converts into any <see cref="Option{T}.None"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IMPL.None None() => default;

    /// <inheritdoc cref="Option{T}.None"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None;

    /// <inheritdoc cref="Option{T}.Some"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    /// <inheritdoc cref="Option{T}.SomeIf"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> SomeIf<T>(T value, Func<T, bool> predicate)
        => Option<T>.SomeIf(value, predicate);

//#if NET9_0_OR_GREATER
//    /// <inheritdoc cref="Option{T}.SomeIf"/>
//    public static RefOption<T> SomeIf<T>(T value, Func<T, bool> predicate,
//        // ReSharper disable once MethodOverloadWithOptionalParameter
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//        => RefOption<T>.SomeIf(value, predicate);
//#endif

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <c>null</c>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> NotNull<T>(T? value)
        where T : class
    {
        if (value is not null)
            return Option<T>.Some(value);
        return Option<T>.None;
    }

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <c>null</c>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once ConvertNullableToShortForm
    public static Option<T> NotNull<T>(Nullable<T> value)
        where T : struct
    {
        if (value.HasValue)
        {
            return Option<T>.Some(value.GetValueOrDefault());
        }

        return Option<T>.None;
    }
#endregion

    public static Option<T> Try<T>(Func<T>? func)
    {
        if (func is null)
            return Option<T>.None;

        try
        {
            return Some<T>(func.Invoke());
        }
        catch
        {
            return Option<T>.None;
        }
    }

    public static Option<T> Try<I, T>(
        [NotNullWhen(true)] I? instance,
        [NotNullWhen(true)] Func<I, T>? instanceFunc)
    {
        if (instance is null || instanceFunc is null)
            return Option<T>.None;

        try
        {
            return Some<T>(instanceFunc.Invoke(instance));
        }
        catch
        {
            return Option<T>.None;
        }
    }

    public static Option<T> Try<T>(TryInvoke<T>? tryInvoke)
    {
        if (tryInvoke is null)
            return Option<T>.None;

        bool ok;
        T? value;

        try
        {
            ok = tryInvoke(out value);
        }
        catch
        {
            return Option<T>.None;
        }

        if (ok)
        {
            return Some<T>(value);

        }
        return Option<T>.None;
    }
}