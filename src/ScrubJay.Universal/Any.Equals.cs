// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public static partial class Any
{
    [OverloadResolutionPriority(1000)]
    public static bool Equals<T>(T? left, T? right, TypeConstraints.RefStruct.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
        {
            return left.Equals(right);
        }
        else if (right is not null)
        {
            return right.Equals(left);
        }
        else
        {
            return true;
        }
    }

    [OverloadResolutionPriority(500)]
    public static bool Equals<T>(T? left, T? right, IEqualityComparer<T>? comparer)
    {
        if (comparer is not null)
        {
            return comparer.Equals(left!, right!);
        }
        return EqualityComparer<T>.Default.Equals(left, right);
    }
    
    [OverloadResolutionPriority(100)]
    public static bool Equals<T>(T? left, T? right, TypeConstraints.Unbounded<T> _ = default)
    {
        return EqualityComparer<T>.Default.Equals(left, right);
    }
    
  

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(10)]
    public static bool Equals<T>(T? left, T? right, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        throw new NotImplementedException();
    }
#endif
}