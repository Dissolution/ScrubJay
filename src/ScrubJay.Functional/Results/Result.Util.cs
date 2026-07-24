using System.Linq.Expressions;

namespace ScrubJay.Functional;

partial struct Result
{
    #region Try
    public static Result<Unit, Exception> Try(Action? action)
    {
        if (action is null)
            return new ArgumentNullException(nameof(action));
        try
        {
            action();
            return default(Unit);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<Unit, Exception> Try<S>(S state, Action<S>? action)
#if NET9_0_OR_GREATER
        where S : allows ref struct
#endif
    {
        if (action is null)
            return new ArgumentNullException(nameof(action));
        try
        {
            action(state);
            return default(Unit);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<R, Exception> Try<R>(Func<R>? func)
    {
        if (func is null)
            return new ArgumentNullException(nameof(func));
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<R, Exception> Try<S, R>(S state, Func<S, R>? func)
#if NET9_0_OR_GREATER
        where S : allows ref struct
#endif
    {
        if (func is null)
            return new ArgumentNullException(nameof(func));
        try
        {
            return func(state);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
    #endregion

    public static Result<T, Exception> NotNull<T>([AllowNull, NotNullWhen(true)] T? value)
        where T : notnull
    {
        if (value is null)
            return new ArgumentNullException(nameof(value));
        return value!;
    }
    
    public static Result<T> From<T>(T value) => Result<T>.Ok(value);
    
    public static Result<T> From<T>(Exception error) => Result<T>.Error(error);

    public static Task<Result<T>> FromAsync<T>(T value) => Task.FromResult(Result<T>.Ok(value));
    
    public static Task<Result<T>> FromAsync<T>(Exception error) => Task.FromResult(Result<T>.Error(error));
}