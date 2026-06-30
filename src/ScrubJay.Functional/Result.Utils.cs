using System.Globalization;

namespace ScrubJay.Functional;

partial struct Result
{
#if NET7_0_OR_GREATER
    public static Result<T> TryParse<T>(
        scoped ReadOnlySpan<char> text,
        IFormatProvider? provider = null)
        where T : ISpanParsable<T>
    {
        if (T.TryParse(text, provider, out var value))
            return value;
        return new ArgumentException($"Could not parse '{text}' to a {typeof(T)} value", nameof(text));
    }

    public static Result<T> TryParse<T>(
        string? str,
        IFormatProvider? provider = null)
        where T : IParsable<T>
    {
        if (T.TryParse(str, provider, out var value))
            return value;
        return new ArgumentException($"Could not parse \"{str}\" to a {typeof(T)} value", nameof(str));
    }

    public static Result<N> TryParse<N>(
        scoped ReadOnlySpan<char> text,
        NumberStyles numberStyle = NumberStyles.Number,
        IFormatProvider? provider = null)
        where N : INumberBase<N>
    {
        if (N.TryParse(text, numberStyle, provider, out var value))
            return value;
        return new ArgumentException($"Could not parse '{text}' to a {typeof(N)} number", nameof(text));
    }

    public static Result<N> TryParse<N>(
        string? str,
        NumberStyles numberStyle = NumberStyles.Number,
        IFormatProvider? provider = null)
        where N : INumberBase<N>
    {
        if (N.TryParse(str, numberStyle, provider, out var value))
            return value;
        return new ArgumentException($"Could not parse \"{str}\" to a {typeof(N)} number", nameof(str));
    }
#endif

    /// <summary>
    /// Try to invoke an <see cref="Action"/>
    /// </summary>
    /// <param name="action"></param>
    /// <returns>
    /// The <see cref="Result{T}"/> of the invocation
    /// </returns>
    public static Result Try(Action? action)
    {
        if (action is null)
        {
            return new ArgumentNullException(nameof(action));
        }

        try
        {
            action.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result Try<I>(
        [NotNullWhen(true)] I? instance,
        [NotNullWhen(true)] Action<I>? instanceAction)
    {
        if (instance is null)
            return new ArgumentNullException(nameof(instance));

        if (instanceAction is null)
            return new ArgumentNullException(nameof(instanceAction));

        try
        {
            instanceAction.Invoke(instance);
            return true;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }


    public static Result<T> Try<T>(Func<T>? func)
    {
        if (func is null)
            return new ArgumentNullException(nameof(func));

        try
        {
            var value = func.Invoke();
            return Result<T>.Ok(value);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T> Try<I, T>(
        [NotNullWhen(true)] I? instance,
        [NotNullWhen(true)] Func<I, T>? instanceFunc)
    {
        if (instance is null)
            return new ArgumentNullException(nameof(instance));

        if (instanceFunc is null)
            return new ArgumentNullException(nameof(instanceFunc));

        try
        {
            var value = instanceFunc.Invoke(instance);
            return Result<T>.Ok(value);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T> Try<T>(TryInvoke<T>? tryInvoke)
    {
        if (tryInvoke is null)
            return new ArgumentNullException(nameof(tryInvoke));

        try
        {
            bool b = tryInvoke(out var value);
            if (b)
                return Result<T>.Ok(value);
            return Result<T>.Error(new InvalidOperationException());
        }
        catch (Exception ex)
        {
            return ex;
        }
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

}