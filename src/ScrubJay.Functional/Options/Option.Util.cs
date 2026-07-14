namespace ScrubJay.Functional;

partial struct Option
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
}