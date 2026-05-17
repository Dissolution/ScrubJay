namespace ScrubJay.Universal.Extensions;

/// <summary>
///
/// </summary>
[PublicAPI]
public static class UniversalExtensions
{
    /// <summary>
    /// Is this <see cref="object"/> a <typeparamref name="T"/> <paramref name="value"/>?
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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

    extension<T>(Nullable<T> nullable)
        where T : struct
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(out T value)
        {
            value = nullable.GetValueOrDefault();
            return nullable.HasValue;
        }
    }
}