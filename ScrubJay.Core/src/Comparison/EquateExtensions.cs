// ReSharper disable InvokeAsExtensionMethod

#pragma warning disable CA1822, IDE0062, CA1708, CA1307

namespace ScrubJay.Comparison;

[PublicAPI]
public static class EquateExtensions
{
#region Textual Types
    // char, text, string?, char[]?

#region char
    extension(char ch)
    {
        public bool Equate(char other) => other == ch;

        public bool Equate(scoped text other)
        {
            if (other.Length == 1)
                return other[0] == ch;
            return false;
        }

        public bool Equate(string? other)
        {
            if (other is not null && other.Length == 1)
                return other[0] == ch;
            return false;
        }

        public bool Equate(char[]? other)
        {
            if (other is not null && other.Length == 1)
                return other[0] == ch;
            return false;
        }
    }

    extension(in char ch)
    {
        public bool Equate(in char other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(ch.AsSpan(), other.AsSpan(), comparison);
        }

        public bool Equate(scoped text other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(ch.AsSpan(), other, comparison);
        }

        public bool Equate(string? other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(ch.AsSpan(), other.AsSpan(), comparison);
        }

        public bool Equate(char[]? other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(ch.AsSpan(), other.AsSpan(), comparison);
        }
    }
#endregion

#region text
    extension(scoped text text)
    {
        public bool Equate(in char other)
        {
            if (text.Length == 1)
                return text[0] == other;
            return false;
        }

        public bool Equate(char[]? other)
        {
            return MemoryExtensions.SequenceEqual<char>(text, other.AsSpan());
        }

        public bool Equate(scoped text other)
        {
            return MemoryExtensions.SequenceEqual<char>(text, other);
        }

        public bool Equate(string? other)
        {
            return MemoryExtensions.SequenceEqual<char>(text, other.AsSpan());
        }

        public bool Equate(in char other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(text, other.AsSpan(), comparison);
        }

        public bool Equate(scoped text other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(text, other, comparison);
        }

        public bool Equate(string? other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(text, other.AsSpan(), comparison);
        }

        public bool Equate(char[]? other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(text, other.AsSpan(), comparison);
        }
    }
#endregion
#region string
    extension(string? str)
    {
        public bool Equate(in char other)
        {
            return str is not null && str.Length == 1 && str[0] == other;
        }

        public bool Equate(scoped text other)
        {
            if (str is null)
                return false;
            return MemoryExtensions.SequenceEqual<char>(str.AsSpan(), other);
        }

        public bool Equate(string? other)
        {
            return string.Equals(str, other, StringComparison.Ordinal);
        }

        public bool Equate(char[]? other)
        {
            if (str is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.SequenceEqual<char>(str.AsSpan(), other.AsSpan());
        }

        public bool Equate(in char other, StringComparison comparison)
        {
            if (str is null)
                return false;
            return MemoryExtensions.Equals(str.AsSpan(), other.AsSpan(), comparison);
        }

        public bool Equate(scoped text other, StringComparison comparison)
        {
            return MemoryExtensions.Equals(str.AsSpan(), other, comparison);
        }

        public bool Equate(string? other, StringComparison comparison)
        {
            return string.Equals(str, other, comparison);
        }

        public bool Equate(char[]? other, StringComparison comparison)
        {
            if (str is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.Equals(str.AsSpan(), other.AsSpan(), comparison);
        }
    }
#endregion

#region char[]?
    extension(char[]? chars)
    {
        public bool Equate(in char other)
        {
            return chars is not null && chars.Length == 1 && chars[0] == other;
        }

        public bool Equate(scoped text other)
        {
            if (chars is null)
                return false;
            return MemoryExtensions.SequenceEqual<char>(chars.AsSpan(), other);
        }

        public bool Equate(string? other)
        {
            if (chars is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.SequenceEqual<char>(chars.AsSpan(), other.AsSpan());
        }

        public bool Equate(char[]? other)
        {
            if (chars is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.SequenceEqual<char>(chars.AsSpan(), other.AsSpan());
        }

        public bool Equate(in char other, StringComparison comparison)
        {
            if (chars is null)
                return false;
            return MemoryExtensions.Equals(chars.AsSpan(), other.AsSpan(), comparison);
        }

        public bool Equate(scoped text other, StringComparison comparison)
        {
            if (chars is null)
                return false;
            return MemoryExtensions.Equals(chars.AsSpan(), other, comparison);
        }

        public bool Equate(string? other, StringComparison comparison)
        {
            if (chars is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.Equals(chars.AsSpan(), other.AsSpan(), comparison);
        }

        public bool Equate(char[]? other, StringComparison comparison)
        {
            if (chars is null)
                return other is null;
            if (other is null)
                return false;
            return MemoryExtensions.Equals(chars.AsSpan(), other.AsSpan(), comparison);
        }
    }
#endregion
#endregion

    extension<T>(T? value)
    {
        public bool Equate(T? other)
        {
            return EqualityComparer<T>.Default.Equals(value!, other!);
        }

        public bool Equate(T? other, IEqualityComparer<T>? comparer)
        {
            return (comparer ?? EqualityComparer<T>.Default).Equals(value!, other!);
        }
    }

    extension(object? obj)
    {
        public bool Equate(object? other)
        {
            return ObjectRelater.Default.Equate(obj, other);
        }

        public bool Equate(object? other, IEqualityComparer? comparer)
        {
            return (comparer ?? ObjectRelater.Default).Equals(obj, other);
        }
    }
}