using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultExtensions
{
    extension<T, E>(Result<T, E>)
        where T : IEquatable<T>
        where E : IEquatable<E>
    {
        public static bool operator ==(Result<T, E> left, Result<T, E> right) => left.Equals(right);
        public static bool operator !=(Result<T, E> left, Result<T, E> right) => left.Equals(right);
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
        [OverloadResolutionPriority(100)]
        public int CompareTo(Result<T, E> other)
        {
            if (result._success)
            {
                if (other._success)
                {
                    return Relate.Compare(result._value!, other._value!);
                }
                else
                {
                    // Ok sorts after Error
                    return 1;
                }
            }
            else
            {
                if (other._success)
                {
                    // Error sorts before Ok
                    return -1;
                }
                else
                {
                    return Relate.Compare(result._error!, other._error!);
                }
            }
        }
    }

    extension<T, E>(Result<T, E> result)
        where T : IFormattable
        where E : IFormattable
    {
        [OverloadResolutionPriority(100)]
        public string ToString(string? format, IFormatProvider? provider = null)
        {
            if (result._success)
            {
                return $"Ok({result._value?.ToString(format, provider)})";
            }
            else
            {
                return $"Error({result._error?.ToString(format, provider)})";
            }
        }
    }

#if NET6_0_OR_GREATER
    extension<T, E>(Result<T, E> result)
        where T : ISpanFormattable
        where E : ISpanFormattable
    {
        [OverloadResolutionPriority(100)]
        public bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = default)
        {
            int capacity = destination.Length;

            if (result.IsOk(out var value, out var error))
            {
                if (capacity >= 4)
                {
                    destination[0] = 'O';
                    destination[1] = 'k';
                    destination[2] = '(';
                    if (value is not null)
                    {
                        if (!value.TryFormat(destination[3..], out var written, format, provider))
                            goto FAIL;
                        if (3 + written >= capacity)
                            goto FAIL;
                        destination[3 + written] = ')';
                        charsWritten = 4 + written;
                        return true;
                    }
                    destination[3] = ')';
                    charsWritten = 4;
                    return true;
                }
            }
            else
            {
                if (capacity >= 7)
                {
                    destination[0] = 'E';
                    destination[1] = 'r';
                    destination[2] = 'r';
                    destination[3] = 'o';
                    destination[4] = 'r';
                    destination[5] = '(';
                    if (value is not null)
                    {
                        if (!value.TryFormat(destination[6..], out var written, format, provider))
                            goto FAIL;
                        if (6 + written >= capacity)
                            goto FAIL;
                        destination[6 + written] = ')';
                        charsWritten = 7 + written;
                        return true;
                    }
                    destination[6] = ')';
                    charsWritten = 7;
                    return true;
                }
            }

            FAIL:
            charsWritten = 0;
            destination.Clear();
            return false;
        }
    }
#endif
}