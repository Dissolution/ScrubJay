namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is<T>(this object? obj, [MaybeNullWhen(false)] out T value)
    {
        // ReSharper disable once MergeCastWithTypeCheck
        if (obj is T)
        {
            value = (T)obj;
            return true;
        }
        value = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool As<T>(this object? obj, out T? value)
    {
        // ReSharper disable once MergeCastWithTypeCheck
        if (obj is T)
        {
            value = (T)obj;
            return true;
        }
        // 'as' also allows 
        value = default;
        return false;
    }
}