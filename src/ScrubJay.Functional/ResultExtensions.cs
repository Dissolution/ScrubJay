using System.Runtime.ExceptionServices;
using ScrubJay.Generics;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultExtensions
{
    extension<T>(Result<T, Exception> result)
    {
        [OverloadResolutionPriority(100)]
        public T OkOrThrow()
        {
            if (result.IsOk(out var value, out var error))
                return value!;
            error ??= new InvalidOperationException($"{result.Display()} is not Ok");
            ExceptionDispatchInfo.Throw(error);
            throw new UnreachableException();
        }

        public void ThrowIfError()
        {
            if (!result._isOk)
            {
                throw result._error!;
            }
        }
    }

#region Equality

    extension<T, E>(Result<T, E>)
        where T : IEquatable<T>
        where E : IEquatable<E>
    {
        public static bool operator ==(Result<T, E> left, Result<T, E> right) => left.Equals(right);
        public static bool operator !=(Result<T, E> left, Result<T, E> right) => left.Equals(right);
    }

    extension<T, E>(Result<T, E> result)
        where T : IEquatable<T>
        where E : IEquatable<E>
    {
        public bool Equals(Result<T, E> other)
        {
            if (result._isOk)
            {
                if (other._isOk)
                {
                    return EqualityComparer<T>.Default.Equals(result._value, other._value);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (other._isOk)
                {
                    return false;
                }
                else
                {
                    return EqualityComparer<E>.Default.Equals(result._error, other._error);
                }
            }
        }
    }

    extension<T, E>(Result<T, E>)
        where T : IEquatable<T>
    {
        public static bool operator ==(Result<T, E> result, T? value) => result._isOk && EqualityComparer<T>.Default.Equals(result._value, value);
        public static bool operator !=(Result<T, E> result, T? value) => !result._isOk || !EqualityComparer<T>.Default.Equals(result._value, value);
    }

    extension<T, E>(Result<T, E> result)
        where T : IEquatable<T>
    {
        [OverloadResolutionPriority(100)]
        public bool Equals(T? value)
        {
            return result._isOk && EqualityComparer<T>.Default.Equals(result._value, value);
        }
    }

    extension<T, E>(Result<T, E>)
        where E : IEquatable<E>
    {
        public static bool operator ==(Result<T, E> result, E? error) => !result._isOk && EqualityComparer<E>.Default.Equals(result._error, error);
        public static bool operator !=(Result<T, E> result, E? error) => result._isOk || !EqualityComparer<E>.Default.Equals(result._error, error);
    }

    extension<T, E>(Result<T, E> result)
        where E : IEquatable<E>
    {
        [OverloadResolutionPriority(100)]
        public bool Equals(E? error)
        {
            return !result._isOk && EqualityComparer<E>.Default.Equals(result._error, error);
        }
    }

#endregion

#region Comparison

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

    extension<T, E>(Result<T, E>)
        where T : IComparable<T>
    {
        public static bool operator <(Result<T, E> result, T? value) => result.CompareTo(value) < 0;
        public static bool operator <=(Result<T, E> result, T? value) => result.CompareTo(value) <= 0;
        public static bool operator >(Result<T, E> result, T? value) => result.CompareTo(value) > 0;
        public static bool operator >=(Result<T, E> result, T? value) => result.CompareTo(value) >= 0;
    }

    extension<T, E>(Result<T, E> result)
        where T : IComparable<T>
    {
        public int CompareTo(T? value)
        {
            if (result._isOk)
            {
                return Comparer<T>.Default.Compare(result._value, value);
            }
            else
            {
                // our error sorts before their ok
                return -1;
            }
        }
    }

    extension<T, E>(Result<T, E>)
        where E : IComparable<E>
    {
        public static bool operator <(Result<T, E> result, E? error) => result.CompareTo(error) < 0;
        public static bool operator <=(Result<T, E> result, E? error) => result.CompareTo(error) <= 0;
        public static bool operator >(Result<T, E> result, E? error) => result.CompareTo(error) > 0;
        public static bool operator >=(Result<T, E> result, E? error) => result.CompareTo(error) >= 0;
    }

    extension<T, E>(Result<T, E> result)
        where E : IComparable<E>
    {
        public int CompareTo(E? error)
        {
            if (!result._isOk)
            {
                return Comparer<E>.Default.Compare(result._error, error);
            }
            else
            {
                // our ok sorts after their error
                return 1;
            }
        }
    }

#endregion

#region Formatting

    extension<T, E>(Result<T, E> result)
        where T : IFormattable
        where E : IFormattable
    {
        [OverloadResolutionPriority(100)]
        public string ToString(string? format, IFormatProvider? provider = null, Constrain.HasIFormattable<T> _ = default, Constrain.HasIFormattable<E> __ = default)
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

    extension<T, E>(Result<T, E> result)
        where T : IFormattable
    {
        [OverloadResolutionPriority(90)]
        public string ToString(string? format, IFormatProvider? provider = null, Constrain.HasIFormattable<T> _ = default)
        {
            if (result._isOk)
            {
                return $"Ok({result._value?.ToString(format, provider)})";
            }
            else
            {
                return $"Error({result._error})";
            }
        }
    }

    extension<T, E>(Result<T, E> result)
        where E : IFormattable
    {
        [OverloadResolutionPriority(80)]
        public string ToString(string? format, IFormatProvider? provider = null, Constrain.HasIFormattable<E> _ = default)
        {
            if (result._isOk)
            {
                return $"Ok({result._value})";
            }
            else
            {
                return $"Error({result._error?.ToString(format, provider)})";
            }
        }
    }

#endregion
}