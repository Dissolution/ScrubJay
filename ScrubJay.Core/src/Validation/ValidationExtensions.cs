namespace ScrubJay.Validation;

[PublicAPI]
public static partial class ValidationExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static T ThrowIfNull<T>([AllowNull, NotNull] this T value,
        [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is not null)
            return value;
        throw new ArgumentNullException(valueName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfNot<T>(this object? obj, [CallerArgumentExpression(nameof(obj))] string? objectName = null)
    {
        if (obj is T)
        {
            return (T)obj;
        }

        throw Ex.Arg(obj, argumentName: objectName, info: $"was not a boxed {typeof(T)} instance");
    }
    
    [return: NotNullIfNotNull(nameof(type))]
    public static object? ThrowIfNot(
        this object? obj, 
        Type? type,
        [CallerArgumentExpression(nameof(obj))] string? objectName = null)
    {
        if (type is null)
        {
            if (obj is null)
                return null;
            throw Ex.Arg(obj, $"was not null", objectName);
        }

        if (obj is null)
            throw Ex.ArgNull(obj, objectName);

        if (obj.GetType().IsAssignableTo(type))
            return obj;

        throw Ex.Arg(obj, $"was not a boxed {type} instance", objectName);
    }
}