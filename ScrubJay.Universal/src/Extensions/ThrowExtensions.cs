namespace ScrubJay.Universal.Extensions;

public static class ThrowExtensions
{
    [DoesNotReturn]
    private static void ThrowArgNull<T>(T? _, string? info, string? argumentName)
    {
        DefaultInterpolatedStringHandler message = new(21, 3);
        message.Write(typeof(T));
        message.Write(" argument '");
        message.Write(argumentName);
        message.Write("' was null");
        if (!info.IsNullOrEmpty())
        {
            message.Write(": ");
            message.Write(info);
        }
        throw new ArgumentNullException(argumentName, message.ToStringAndClear());
    }

    public static T ThrowIfNull<T>(
        [AllowNull, NotNull] this T? value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : class
    {
        if (value is null)
            ThrowArgNull<T>(value, info, argumentName);
        return value;
    }

    public static T ThrowIfNull<T>(
        [AllowNull, NotNull] this Nullable<T> value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : struct
    {
        if (!value.HasValue)
            ThrowArgNull<T?>(value, info, argumentName);
        return value.GetValueOrDefault();
    }
}