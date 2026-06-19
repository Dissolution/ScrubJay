using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultExtensions
{
//    extension<T, E>(Result<T, E>)
//        where T : IEquatable<T>
//        where E : IEquatable<E>
//    {
//        public static bool operator ==(in Result<T, E> left, in Result<T, E> right)
//        {
//            return ResultExtensions.Equals(in left, in right);
//        }
//
//        public static bool operator !=(in Result<T, E> left, in Result<T, E> right)
//        {
//            return !ResultExtensions.Equals(in left, in right);
//        }
//    }
//
//    extension<T, E>(ref readonly Result<T, E> result)
//        where T : IEquatable<T>
//        where E : IEquatable<E>
//    {
//        public bool Equals(ref readonly Result<T, E> other)
//        {
//            if (result.IsOk(out var rValue, out var rError))
//            {
//                return other.IsOk(out var oValue) && Prelude.Equate(rValue, oValue);
//            }
//            else
//            {
//                return other.IsError(out var oError) && Prelude.Equate(rError, oError);
//            }
//        }
//    }
//    
//    extension<T, E>(Result<T, E>)
//        where T : IEquatable<T>
//    {
//        public static bool operator ==(in Result<T, E> result, in T value)
//        {
//            return ResultExtensions.Equals(in result, in value);
//        }
//
//        public static bool operator !=(Result<T, E> result, in T value) 
//        {
//            return !ResultExtensions.Equals(in result, in value);
//        }
//    }

    [OverloadResolutionPriority(10)]
    public static bool Equals<T, E>(this ref readonly Result<T, E> result, T other)
        where T : IEquatable<T>
    {
        return result.IsOk(out var rValue) && Prelude.Equate(rValue, other);
    }

    [OverloadResolutionPriority(10)]
    public static int CompareTo<T, E>(this ref readonly Result<T, E> result, T other)
        where T : IComparable<T>
    {
        if (result.IsOk(out var rValue))
        {
            return Prelude.Compare(rValue, other);
        }
        return 0; // cannot be compared
    }
}