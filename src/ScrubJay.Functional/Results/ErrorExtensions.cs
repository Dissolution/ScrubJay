using ScrubJay.Polyfills;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ErrorExtensions
{
    extension<T, E>(Result<T, E>)
        where E : IEquatable<E>
    {
        public static bool operator ==(Result<T, E> result, T other) => result.Equals(other);
        public static bool operator !=(Result<T, E> result, T other) => !result.Equals(other);
    }

    extension<T, E>(Result<T, E>)
        where E : IComparable<E>
    {
        public static bool operator <(Result<T, E> result, E other) => result.CompareTo(other) < 0;
        public static bool operator <=(Result<T, E> result, E other) => result.CompareTo(other) <= 0;
        public static bool operator >(Result<T, E> result, E other) => result.CompareTo(other) > 0;
        public static bool operator >=(Result<T, E> result, E other) => result.CompareTo(other) >= 0;
    }

    extension<T, E>(Result<T, E> result)
        where E : IComparable<E>
    {
        [OverloadResolutionPriority(75)]
        public int CompareTo(E? other)
        {
            if (!result._success)
            {
                return Any.Compare(result._error, other);
            }
            return 0;
        }

        [OverloadResolutionPriority(75)]
        public int CompareTo(Result<T, E> other, TypeConstraints.HasIComparable<E, E> _ = default)
        {
            if (!result._success && !other._success)
            {
                return Any.Compare(result._error, other._error);
            }
            return 0;
        }

        [OverloadResolutionPriority(50)]
        public int CompareTo<OtherT>(Result<OtherT, E> other, TypeConstraints.HasIComparable<E, E> _ = default)
        {
            if (!result._success && !other._success)
            {
                return Any.Compare(result._error, other._error);
            }
            return 0;
        }
    }

    extension<T, E>(Result<T, E> result)
        where E : IFormattable
    {
        [OverloadResolutionPriority(75)]
        public string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            if (result._success)
            {
                return $"Ok({result._value})";
            }
            else
            {
                return $"Error({result._error?.ToString(format, formatProvider)})";
            }
        }
    }
}