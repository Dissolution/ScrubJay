global using ScrubJay.Functional.Extensions;
using ScrubJay.Functional.IMPL;


namespace ScrubJay.Functional;

/// <summary>
/// A <c>static</c> prelude to import common functions.
/// </summary>
/// <remarks>
/// To include these methods in a single <c>.cs</c> file, add to its <c>usings</c> section:
/// <code>
/// using static ScrubJay.Functional.Prelude;
/// </code><br/>
/// To include them in an entire project, add to its <c>.csproj</c> file:
/// <code>
/// &lt;ItemGroup&gt;
///     &lt;Using Include="ScrubJay.Functional.Prelude" Static="true"/&gt;
/// &lt;/ItemGroup&gt;
/// </code>
/// </remarks>
[PublicAPI]
public static class Prelude
{
    /// <summary>
    /// Returns the <see cref="IMPL.None"/> value.
    /// </summary>
    public static None None
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => field;
    } = None.Default;

    /// <summary>
    /// Returns an <see cref="Option{T}.Some"/> containing the given <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    /// <summary>
    /// Returns an <see cref="Ok{T}"/> that implicitly converts into a <see cref="Result{T}.Ok"/>, <see cref="Result{T,E}.Ok"/>, or <see cref="Option{T}.Some"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ok<T> Ok<T>(T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new Ok<T>(value);

    /// <summary>
    /// Returns an <see cref="Error{E}"/> that implicitly converts into a <see cref="Result{T,E}.Error"/>.
    /// </summary>
    /// <param name="error"></param>
    /// <typeparam name="E"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error<E> Error<E>(E error)
#if NET9_0_OR_GREATER
        where E : allows ref struct
#endif
        => new Error<E>(error);

    /// <summary>
    /// Returns the <see cref="Unit"/> value.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unit Unit() => default(Unit);

    /// <summary>
    /// Tries to execute an <see cref="Action"/> and returns a <see cref="Result"/> describing its invocation.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Try(Action? action)
    {
        return Result.Try(action);
    }

    /// <summary>
    /// Tries to execute an <see cref="Func{T}"/> and returns a <see cref="Result{T}"/> describing its invocation.
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Try<T>(Func<T>? func)
    {
        return Result.Try<T>(func);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Try<S, R>(S state, Func<S, R>? func, R fallback)
#if NET9_0_OR_GREATER
        where S : allows ref struct
        where R : allows ref struct
#endif
    {
        if (func is null)
            return fallback;

        try
        {
            return func.Invoke(state);
        }
        catch // ignore all exceptions
        {
            return fallback;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Try<R>(Func<R>? func, R fallback)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (func is null)
            return fallback;

        try
        {
            return func.Invoke();
        }
        catch // ignore all exceptions
        {
            return fallback;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swallow(Action? action)
    {
        try
        {
            if (action is not null)
            {
                action.Invoke();
            }
        }
        catch
        {
            // ignore all exceptions, as it says
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swallow<R>(Func<R>? func)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        try
        {
            if (func is not null)
            {
                _ = func.Invoke();
            }
        }
        catch
        {
            // ignore all exceptions, as it says
        }
    }
}