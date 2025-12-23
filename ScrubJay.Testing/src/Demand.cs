namespace ScrubJay.Testing;

[PublicAPI]
public static partial class Demand
{
    public static Actual<T> That<T>(T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new Actual<T>(value, valueName);
    }
}