namespace ScrubJay.Errors.Validation;

public static class ThrowExtensions
{
    public static T ThrowIfNull<T>(
        [AllowNull, NotNull] this T? value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : class
    {
        if (value is null)
            Throw.ArgNull<T>(value, info, argumentName);
        return value;
    }

    public static T ThrowIfNull<T>(
        [AllowNull,NotNull] this Nullable<T> value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : struct
    {
        if (!value.HasValue)
            Throw.ArgNull<T?>(value, info, argumentName);
        return value.GetValueOrDefault();
    }

    public static T ThrowIfNot<T>(
        this object? obj,
        string? info = null,
        [CallerArgumentExpression(nameof(obj))]
        string? objectName = null)
    {
        if (obj is not T)
            Throw.Arg(obj, info, objectName);
        return (T)obj;
    }
}