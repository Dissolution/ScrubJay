// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Generics;

public static partial class Anything
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(T? left, T? right) => Comparer<T>.Default.Compare(left, right);


    //internal static bool EquateImpl<T>(in T? left, in T? right)
    //    where T : IEquatable<T>
    //{
    //    if (left is not null)
    //        return left.Equals(right);
    //    if (right is not null)
    //        return right.Equals(left);
    //    return true; // null == null
    //}

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right)
        where T : allows ref struct
    {
        return AnythingCompareCache<T>.Invoke(in left, in right);
    }

    internal delegate int AnythingCompare<T>(in T? left, in T? right)
        where T : allows ref struct;

    internal static class AnythingCompareCache<T>
        where T : allows ref struct
    {
        public static readonly AnythingCompare<T> Invoke = CreateDelegate();

        private static AnythingCompare<T> CreateDelegate()
        {
            var type = typeof(T);
            throw new NotImplementedException();
        }
    }

#endif
}