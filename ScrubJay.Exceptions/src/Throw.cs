namespace ScrubJay.Exceptions;

[PublicAPI]
public static class Throw
{
    public static void IfNull<T>([AllowNull, NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is not null)
            return;
        var arg = Argument.Capture<T>(in value, valueName);
        throw new ArgNullException(arg);
    }
}