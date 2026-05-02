namespace ScrubJay.Exceptions;

partial class Ex
{
    public static ArgNullException ArgNull<T>(
        T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = new Argument(typeof(T), argumentName, null);
        return new ArgNullException(arg, message);
    }
    
    public static ArgNullException ArgNull(
        object? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = new Argument(null, argumentName, null);
        return new ArgNullException(arg, message);
    }

    public static ArgNullException ArgNull(string argumentName, string? message = null)
    {
        var arg = new Argument(null, argumentName, null);
        return new ArgNullException(arg, message);
    }
}