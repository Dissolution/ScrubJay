namespace ScrubJay.Functional;

[PublicAPI]
public static class OptionExtensions
{
    extension<T>(Option<T>)
        where T : IComparable<T>
    {
        public static bool operator >(Option<T> left, Option<T> right) => left.CompareTo(right) > 0;
        public static bool operator >=(Option<T> left, Option<T> right) => left.CompareTo(right) >= 0;
        public static bool operator <(Option<T> left, Option<T> right) => left.CompareTo(right) < 0;
        public static bool operator <=(Option<T> left, Option<T> right) => left.CompareTo(right) <= 0;
    }

    extension<T>(ref readonly Option<T> option)
        where T : IComparable<T>
    {
        public int CompareTo(Option<T> other)
        {
            if (option._isSome)
            {
                if (other._isSome)
                {
                    return Comparer<T>.Default.Compare(option._value, other._value);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (other._isSome)
                {
                    return -1;
                }
                else
                {
                    return 0; // None == None
                }
            }
        }
    }

    extension<T>(ref readonly Option<T> option)
        where T : IFormattable
    {
        public string ToString(string? format, IFormatProvider? provider = null)
        {
            if (option._isSome)
            {
                return option._value?.ToString(format, provider) ?? string.Empty;
            }
            return nameof(Option.None);
        }
    }
}