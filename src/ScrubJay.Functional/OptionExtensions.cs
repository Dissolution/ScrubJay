namespace ScrubJay.Functional;

[PublicAPI]
public static class OptionExtensions
{
    extension<T>(Option<T> option)
        where T : struct
    {
        public Nullable<T> ToNullable()
        {
            if (option._isSome)
                return option._value!;
            return null;
        }
    }

  

#region Equatable

    extension<T>(Option<T>)
        where T : IEquatable<T>
    {
        [OverloadResolutionPriority(100)]
        public static bool operator ==(Option<T> option, T? value) => option._isSome && EqualityComparer<T>.Default.Equals(option._value, value);
        
        [OverloadResolutionPriority(100)]
        public static bool operator !=(Option<T> option, T? value) => !option._isSome || !EqualityComparer<T>.Default.Equals(option._value, value);
    }

    extension<T>(Option<T> option)
        where T : IEquatable<T>
    {
        [OverloadResolutionPriority(90)]
        public bool Equals(T? value)
        {
            return option._isSome && EqualityComparer<T>.Default.Equals(option._value, value);
        }
    }

#endregion

#region Comparision

    extension<T>(Option<T>)
        where T : IComparable<T>
    {
        [OverloadResolutionPriority(100)]
        public static bool operator >(Option<T> left, Option<T> right) => left.CompareTo(right) > 0;
        [OverloadResolutionPriority(100)]
        public static bool operator >=(Option<T> left, Option<T> right) => left.CompareTo(right) >= 0;
        [OverloadResolutionPriority(100)]
        public static bool operator <(Option<T> left, Option<T> right) => left.CompareTo(right) < 0;
        [OverloadResolutionPriority(100)]
        public static bool operator <=(Option<T> left, Option<T> right) => left.CompareTo(right) <= 0;
    }

    extension<T>(Option<T> option)
        where T : IComparable<T>
    {
        [OverloadResolutionPriority(90)]
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

#endregion

#region Formatting

    extension<T>(Option<T> option)
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

    extension<T>(Option<T> option)
        where T : ISpanFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = null)
        {
            if (option._isSome)
            {
                if (option._value is null)
                {
                    charsWritten = 0;
                    return true;
                }

                return option._value.TryFormat(destination, out charsWritten, format, provider);
            }

            return ((ISpanFormattable)default(Option.None)).TryFormat(destination, out charsWritten, format, provider);
        }
    }

#endregion
}