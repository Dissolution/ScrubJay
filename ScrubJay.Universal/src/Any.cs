namespace ScrubJay.Universal;

[PublicAPI]
public static partial class Any
{
    public static int CompareTo<T>(T? value, T? other)
    {
        if (value is IComparable<T> comparable)
            return comparable.CompareTo(other!);
        return Comparer<T>.Default.Compare(value!, other!);
    }

    public static bool Equals<T>(T? value, object? obj)
    {
        if (value is null)
            return obj is null;
        return value.Equals(obj);
    }

    public static bool Equals<T>(T? value, T? other)
    {
        if (value is IEquatable<T> equatable)
            return equatable.Equals(other!);
        return EqualityComparer<T>.Default.Equals(value!, other!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value)
    {
        if (value is null)
            return 0;
        return value.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static Type? GetType<T>(T? value)
    {
        if (value is not null) 
            return value.GetType();
        return null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryBox<T>(T? value, [NotNullIfNotNull(nameof(value))] out object? boxed)
    {
        boxed = (object?)value;
        return true;
    }
}