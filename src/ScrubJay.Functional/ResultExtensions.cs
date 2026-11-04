namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultExtensions
{
    extension<T>(Result<T, Exception> result)
    {
        public void ThrowIfError()
        {
            if (!result._isOk)
            {
                throw result._error!;
            }
        }
    }

    extension<T, E>(Result<T, E>)
        where T : IComparable<T>
        where E : IComparable<E>
    {
        public static bool operator <(Result<T, E> left, Result<T, E> right) => left.CompareTo(right) < 0;
        public static bool operator <=(Result<T, E> left, Result<T, E> right) => left.CompareTo(right) <= 0;
        public static bool operator >(Result<T, E> left, Result<T, E> right) => left.CompareTo(right) > 0;
        public static bool operator >=(Result<T, E> left, Result<T, E> right) => left.CompareTo(right) >= 0;
    }

    extension<T, E>(Result<T, E> result)
        where T : IComparable<T>
        where E : IComparable<E>
    {
        public int CompareTo(Result<T, E> other)
        {
            if (result._isOk)
            {
                if (other._isOk)
                {
                    return Comparer<T>.Default.Compare(result._value, other._value);
                }
                else
                {
                    // our ok sorts after their error
                    return 1;
                }
            }
            else
            {
                if (other._isOk)
                {
                    // our error sorts before their ok
                    return -1;
                }
                else
                {
                    return Comparer<E>.Default.Compare(result._error, other._error);
                }
            }
        }
    }

    extension<T, E>(Result<T, E> result)
        where T : IFormattable
        where E : IFormattable
    {
        public string ToString(string? format, IFormatProvider? provider = null)
        {
            if (result._isOk)
            {
                return $"Ok({result._value?.ToString(format, provider)})";
            }
            else
            {
                return $"Error({result._error?.ToString(format, provider)})";
            }
        }
    }
}