namespace StandaloneSandbox;

public static partial class Demand
{
    public static T IsEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (EqualityComparer<T>.Default.Equals(value!, other!))
            return value;
        throw new ArgumentException($"was not equal to {other}", valueName);
    }

    public static T IsNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(value!, other!))
            return value;
        throw new ArgumentException($"was equal to {other}", valueName);
    }
}