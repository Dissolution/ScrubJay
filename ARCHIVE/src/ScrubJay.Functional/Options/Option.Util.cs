namespace ScrubJay.Functional;

[PublicAPI]
public static class Option
{
    public static Option<T> NotNull<T>([AllowNull, NotNullWhen(true)] T? value)
        where T : class
    {
        if (value is null)
            return default;
        return value!;
    }
    
    public static Option<T> NotNull<T>([AllowNull, NotNullWhen(true)] Nullable<T> nullable)
        where T : struct
    {
        if (nullable.HasValue)
            return Some(nullable.GetValueOrDefault());
        return default;
    }

    public static Option<T> Is<T>([AllowNull, NotNullWhen(true)] object? obj)
    {
        if (obj is T value)
        {
            return Some<T>(value);
        }
        return default;
    }
}