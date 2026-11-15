namespace ScrubJay.Comparison;

[PublicAPI]
public static partial class Relate
{
    [OverloadResolutionPriority(100)]
    public static bool IsNull<C>(
        [AllowNull, NotNullWhen(false)] C? instance)
        where C : class
    {
        return instance is null;
    }

    [OverloadResolutionPriority(90)]
    public static bool IsNull<N>(
        [AllowNull, NotNullWhen(false)] Nullable<N> nullable)
        where N : struct
        => !nullable.HasValue;

    [OverloadResolutionPriority(50)]
    public static bool IsNull<T>([AllowNull, NotNullWhen(false)] ref readonly T? reference)
    {
        return Unsafe.IsNullRef(in reference);
    }

    [OverloadResolutionPriority(10)]
    public static bool IsNull([AllowNull, NotNullWhen(false)] object? obj) => obj is null;
}