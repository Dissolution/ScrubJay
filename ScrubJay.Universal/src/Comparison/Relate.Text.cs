// ReSharper disable InvokeAsExtensionMember
#pragma warning disable CA1307

using ScrubJay.Universal.Extensions;


namespace ScrubJay.Universal.Comparison;

public static partial class Relate
{
    #region char, char
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, char right) => left == right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, char right, StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
            return left == right;
        return MemoryExtensions.Equals(left.AsSpan(), right.AsSpan(), comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, char right, IEqualityComparer<char>? charComparer)
    {
        if (charComparer is null)
            return left == right;
        return charComparer.Equals(left, right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, char right, IEqualityComparer<string>? stringComparer)
    {
        if (stringComparer is null)
            return left == right;
        return stringComparer.Equals(left.ToString(), right.ToString());
    }
    #endregion /char,char

    #region char, string
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, string? right) => right is not null && right.Length == 1 && right[0] == left;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, string? right, StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
            return Equal(left, right);
        return MemoryExtensions.Equals(left.AsSpan(), right.AsSpan(), comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, string? right, IEqualityComparer<char>? charComparer)
    {
        if (charComparer is null)
            return Equal(left, right);
        return right is not null && right.Length == 1 && charComparer.Equals(right[0], left);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, string? right, IEqualityComparer<string>? stringComparer)
    {
        if (stringComparer is null)
            return Equal(left, right);
        return stringComparer.Equals(left.ToString(), right!);
    }
    #endregion

    #region char, text
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, scoped text right) => right.Length == 1 && right[0] == left;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, scoped text right, StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
            return Equal(left, right);
        return MemoryExtensions.Equals(left.AsSpan(), right, comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, scoped text right, IEqualityComparer<char>? charComparer)
    {
        if (charComparer is null)
            return Equal(left, right);
        return right.Length == 1 && charComparer.Equals(right[0], left);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(char left, scoped text right, IEqualityComparer<string>? stringComparer)
    {
        if (stringComparer is null)
            return Equal(left, right);
        return stringComparer.Equals(left.ToString(), right.ToString());
    }
    #endregion

}