namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Functional extensions
/// </summary>
[PublicAPI]
public static class FunctionalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Is<T>(this object? obj)
    {
        // ReSharper disable once MergeCastWithTypeCheck
        if (obj is T)
        {
            return Some((T)obj);
        }
        return None;
    }
}