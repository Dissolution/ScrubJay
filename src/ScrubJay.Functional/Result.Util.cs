using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Functional;

partial struct Result
{
    public static Result<Unit, Exception> Try(Action? action)
    {
        if (action is null)
            return ArgNullException.Create(action);
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
            return ArgNullException.Create(action);
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
            return ArgNullException.Create(func);
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
            return ArgNullException.Create(func);
        try
        {
            return func(state);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T, Exception> NotNull<T>([AllowNull, NotNullWhen(true)] T? value)
        where T : notnull
    {
        if (value is null)
            return ArgNullException.Create(value);
        return value!;
    }
}