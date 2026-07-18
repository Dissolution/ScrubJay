using ScrubJay.Polyfills.Comparison;

namespace ScrubJay.Functional;

[PublicAPI]
public static class SomeExtensions
{
    extension<T>(Option<T>)
        where T : IEquatable<T>
    {
        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
        public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);
    }
    
    extension<T>(Option<T> option)
        where T : IEquatable<T>
    {
        public bool Equate(Option<T> other)
        {
            if (option._some)
            {
                return other._some && Relate.Equate(option._value, other._value);
            }
            return !other._some;
        }
    }
    
    
    extension<T>(Option<T>)
        where T : IComparable<T>
    {
        public static bool operator <(Option<T> left, Option<T> right) => left.CompareTo(right) < 0;
        public static bool operator <=(Option<T> left, Option<T> right) => left.CompareTo(right) <= 0;
        public static bool operator >(Option<T> left, Option<T> right) => left.CompareTo(right) > 0;
        public static bool operator >=(Option<T> left, Option<T> right) => left.CompareTo(right) >= 0;
    }
    
    extension<T>(Option<T> option)
        where T : IComparable<T>
    {
        public int CompareTo(Option<T> other)
        {
            if (option._some)
            {
                if (other._some)
                {
                    return Relate.Compare(option._value, other._value);
                }
                else
                {
                    // some > none
                    return 1;
                }
            }
            else
            {
                if (other._some)
                {
                    // none < some
                    return -1;
                }
                else
                {
                    // none == none
                    return 0;
                }
            }
        }
    }

    extension<T>(Option<T> option)
        where T : IFormattable
    {
        public string ToString(string? format, IFormatProvider? provider = null)
        {
            if (option._some)
            {
                return option._value?.ToString(format, provider) ?? string.Empty; 
            }
            return nameof(None);
        }
    }
}