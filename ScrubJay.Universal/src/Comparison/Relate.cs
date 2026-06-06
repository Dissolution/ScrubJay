// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal.Comparison;

[PublicAPI]
public static partial class Relate
{
#region Equal
#region Value
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal<T>(in T? left, in T? right)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return Any.Equals<T>(in left, in right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal<T>(in T? left, in T? right, IEqualityComparer<T>? equalityComparer)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (equalityComparer is null)
            return Any.Equals<T>(in left, in right);
        return equalityComparer.Equals(left!, right!);
    }
#endregion /Equal Value

#region Text
   
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(scoped text left, scoped text right)
    {
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(scoped text left, scoped text right, IEqualityComparer<char>? charEqualityComparer)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual<char>(left, right, charEqualityComparer);
#else
        if (left.Length != right.Length)
        {
            return false;
        }

        charEqualityComparer ??= EqualityComparer<char>.Default;

        for (int i = 0; i < left.Length; i++)
        {
            if (!charEqualityComparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(string? left, scoped text right)
    {
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(string? left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(string? left, scoped text right, IEqualityComparer<char>? charEqualityComparer)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual<char>(left, right, charEqualityComparer);
#else
        if (left.Length != right.Length)
        {
            return false;
        }

        charEqualityComparer ??= EqualityComparer<char>.Default;

        for (int i = 0; i < left.Length; i++)
        {
            if (!charEqualityComparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }
    
    

  
#endregion /Equal Text

#region Many
#if NET6_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool Equal<T>(scoped ReadOnlySpan<T> left, scoped ReadOnlySpan<T> right)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual(left, right);
#else
        if (left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }

#if NET6_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool Equal<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IEqualityComparer<T>? itemComparer)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual(left, right, itemComparer);
#else
        if (itemComparer is null)
            return Equal<T>(left, right);

        if (left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!itemComparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }

    public static bool Equal<E, T>(E? left, E? right)
        where E : IEnumerable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
        where T : allows ref struct
#endif
    {
        if (left is null)
            return right is null;
        if (right is null)
            return false;

        using var l = left.GetEnumerator();
        using var r = right.GetEnumerator();
        while (true)
        {
            var lMoved = l.MoveNext();
            var rMoved = r.MoveNext();
            if (lMoved != rMoved)
                return false;
            if (!lMoved)
                return true;
            if (!Any.Equals<T>(l.Current, r.Current))
                return false;
        }
    }

    public static bool Equal<E, T>(E? left, E? right, IEqualityComparer<T>? itemComparer)
        where E : IEnumerable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
        where T : allows ref struct
#endif
    {
        if (itemComparer is null)
            return Equal<E, T>(left, right);

        if (left is null)
            return right is null;
        if (right is null)
            return false;

        using var l = left.GetEnumerator();
        using var r = right.GetEnumerator();
        while (true)
        {
            var lMoved = l.MoveNext();
            var rMoved = r.MoveNext();
            if (lMoved != rMoved)
                return false;
            if (!lMoved)
                return true;
            if (!itemComparer.Equals(l.Current, r.Current))
                return false;
        }
    }
#endregion /Equal Many
#endregion /Equal
}