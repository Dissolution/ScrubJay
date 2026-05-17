namespace ScrubJay.Errors.Validation;

public partial class Demand
{
    public static T ThrowIfNull<T>(
        [AllowNull, JetBrains.Annotations.NotNull] this T? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : class
    {
        if (value is null)
            ThrowArgNull<T>(value, null, argumentName);
        return value;
    }

    public static T ThrowIfNull<T>(
        [AllowNull, JetBrains.Annotations.NotNull] this Nullable<T> value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : struct
    {
        if (!value.HasValue)
            ThrowArgNull(value, null, argumentName);
        return value.GetValueOrDefault();
    }

    public static T ThrowIfNot<T>(
        this object? obj,
        [CallerArgumentExpression(nameof(obj))]
        string? objectName = null)
    {
        if (obj is not T)
        {
            // todo: turn to private throw method
            throw Ex.Convert(obj, typeof(T), null, objectName);
        }
        return (T)obj;
    }
}