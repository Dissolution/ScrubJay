#pragma warning disable CA1720

namespace ScrubJay.Collections.Iteration;

public static class Iterate
{
    public static EmptyIterable Empty() => default;

    public static EmptyIterable<T> Empty<T>() => default;

    public static SingleIterable<T> Single<T>(T value) => new SingleIterable<T>(value);
    
    public static IEnumerable<T> WhileNotNull<T>(T? value, Func<T, T?> getNextValue)
        where T : class
    {
        while (value is not null)
        {
            yield return value;

            value = getNextValue(value);
        }
    }
}