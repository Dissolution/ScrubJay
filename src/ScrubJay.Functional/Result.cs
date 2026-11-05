namespace ScrubJay.Functional;

[PublicAPI]
public static partial class Result
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
        where S : allows ref struct
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
        where S : allows ref struct
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

}