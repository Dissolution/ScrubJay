// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class NullableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this Nullable<T> nullable)
        where T : struct
    {
        return nullable.HasValue;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this T? nullable)
        where T : class
    {
        return nullable is not null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this T? nullable,
        TypeConstraints.Unbounded<T> _ = default)
    {
        return nullable is not null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this Nullable<T> nullable, 
        [NotNullWhen(true), MaybeNullWhen(false)] 
        out T nonNullValue)
        where T : struct
    {
        if (nullable.HasValue)
        {
            nonNullValue = nullable.GetValueOrDefault();
            return true;
        }
        nonNullValue = default;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this T? nullable,
        [NotNullWhen(true), MaybeNullWhen(false)] 
        out T nonNullValue)
        where T : class
    {
        nonNullValue = nullable;
        return nullable is not null;
    }
  
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)] 
        this T? nullable,
        [NotNullWhen(true), MaybeNullWhen(false)] 
        out T nonNullValue,
        TypeConstraints.Unbounded<T> _ = default)
    {
        nonNullValue = nullable;
        return nullable is not null;
    }
}