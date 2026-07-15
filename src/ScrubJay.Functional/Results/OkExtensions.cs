using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Comparison;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public static class OkExtensions
{
    extension<T, E>(Result<T, E>)
        where T : IEquatable<T>
    {
        public static bool operator ==(Result<T, E> result, T other) => result.Equals(other);
        public static bool operator !=(Result<T, E> result, T other) => !result.Equals(other);
    }

    extension<T, E>(Result<T, E>)
        where T : IComparable<T>
    {
        public static bool operator <(Result<T, E> result, T other) => result.CompareTo(other) < 0;
        public static bool operator <=(Result<T, E> result, T other) => result.CompareTo(other) <= 0;
        public static bool operator >(Result<T, E> result, T other) => result.CompareTo(other) > 0;
        public static bool operator >=(Result<T, E> result, T other) => result.CompareTo(other) >= 0;
    }

    extension<T, E>(Result<T, E> result)
        where T : IComparable<T>
    {
        [OverloadResolutionPriority(75)]
        public int CompareTo(T? other)
        {
            if (result._isOk)
            {
                return Relate.Compare(result._value, other);
            }
            return 0;
        }

        [OverloadResolutionPriority(75)]
        public int CompareTo(Result<T, E> other, TypeConstraints.HasIComparable<T, T> _ = default)
        {
            if (result._isOk && other._isOk)
            {
                return Relate.Compare(result._value!, other._value!);
            }
            return 0;
        }

        [OverloadResolutionPriority(50)]
        public int CompareTo<OtherE>(Result<T, OtherE> other, TypeConstraints.HasIComparable<T, T> _ = default)
        {
            if (result._isOk && other._isOk)
            {
                return Relate.Compare(result._value!, other._value!);
            }
            return 0;
        }
    }

    extension<T, E>(Result<T, E> result)
        where T : IFormattable
    {
        [OverloadResolutionPriority(75)]
        public string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            if (result._isOk)
            {
                return $"Ok({result._value?.ToString(format, formatProvider)})";
            }
            else
            {
                return $"Error({result._error})";
            }
        }
    }
}