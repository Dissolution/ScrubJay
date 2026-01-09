#nullable enable
namespace StandaloneSandbox;


public static partial class Validate
{
    public static Result<T> IsEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (EqualityComparer<T>.Default.Equals(value!, other!))
            return value;
        return new ArgumentException($"was not equal to {other}", valueName);
    }

    public static Result<T> IsNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(value!, other!))
            return value;
        return new ArgumentException($"was equal to {other}", valueName);
    }
}