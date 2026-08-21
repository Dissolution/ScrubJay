namespace ScrubJay.Functional;

[PublicAPI]
public static class Result
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IMPL.Ok<T> Ok<T>(T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new IMPL.Ok<T>(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IMPL.Error<E> Error<E>(E error)
#if NET9_0_OR_GREATER
        where E : allows ref struct
#endif
        => new IMPL.Error<E>(error);
    
#region Try
    public static Result<Unit> Try(Action? action)
    {
        if (action is null)
        {
            return new ArgumentNullException(nameof(action));
        }

        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            return Result<Unit>.Error(ex);
        }
        return Result<Unit>.Ok(default);
    }

    public static Result<Unit> Try<TState>(TState state,
        [NotNullWhen(true)] Action<TState>? instanceAction)
    {
        if (instanceAction is null)
            return new ArgumentNullException(nameof(instanceAction));

        try
        {
            instanceAction.Invoke(state);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Error(ex);
        }
        return Result<Unit>.Ok(default);
    }

    public static Result<T> Try<T>(Func<T>? func)
    {
        if (func is null)
            return new ArgumentNullException(nameof(func));

        T value;
        try
        {
            value = func.Invoke();
        }
        catch (Exception ex)
        {
            return Result<T>.Error(ex);
        }
        return Result<T>.Ok(value);
    }

    public static Result<T> Try<TState, T>(TState state,
        [NotNullWhen(true)] Func<TState, T>? instanceFunc)
    {
        if (instanceFunc is null)
            return new ArgumentNullException(nameof(instanceFunc));

        T value;
        try
        {
            value = instanceFunc.Invoke(state);
        }
        catch (Exception ex)
        {
            return Result<T>.Error(ex);
        }
        return Result<T>.Ok(value);
    }

    public static Result<T> Try<T>(TryInvoke<T>? tryInvoke)
    {
        if (tryInvoke is null)
            return new ArgumentNullException(nameof(tryInvoke));

        bool invoked;
        T value;
        try
        {
            invoked = tryInvoke(out value);
        }
        catch (Exception ex)
        {
            return Result<T>.Error(ex);
        }
        if (invoked)
        {
            return Result<T>.Ok(value);
        }
        return Result<T>.Error(new InvalidOperationException($"Invocation of {TypeAlias.For<TryInvoke<T>>()} returned false"));
    }
#endregion
}