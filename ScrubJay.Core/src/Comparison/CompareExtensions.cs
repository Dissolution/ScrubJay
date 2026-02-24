// ReSharper disable InvokeAsExtensionMethod

#pragma warning disable CA1822, CA1708, CA1307

namespace ScrubJay.Comparison;

[PublicAPI]
public static class CompareExtensions
{
    extension(in char ch)
    {
        public int Compare(in char other)
            => ch.CompareTo(other);

        public int Compare(char[]? other)
            => ch.AsSpan().Compare(other.AsSpan());

        public int Compare(scoped text other)
            => ch.AsSpan().Compare(other);

        public int Compare(string? other)
            => ch.AsSpan().Compare(other.AsSpan());

        public int Compare(in char other, StringComparison comparison)
            => ch.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(char[]? other, StringComparison comparison)
            => ch.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(scoped text other, StringComparison comparison)
            => ch.AsSpan().Compare(other, comparison);

        public int Compare(string? other, StringComparison comparison)
            => ch.AsSpan().Compare(other.AsSpan(), comparison);
    }

    extension(char[]? chars)
    {
        public int Compare(in char other)
            => chars.AsSpan().Compare(other.AsSpan());

        public int Compare(char[]? other)
            => chars.AsSpan().Compare(other.AsSpan());

        public int Compare(scoped text other)
            => chars.AsSpan().Compare(other);

        public int Compare(string? other)
            => chars.AsSpan().Compare(other.AsSpan());

        public int Compare(in char other, StringComparison comparison)
            => chars.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(char[]? other, StringComparison comparison)
            => chars.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(scoped text other, StringComparison comparison)
            => chars.AsSpan().Compare(other, comparison);

        public int Compare(string? other, StringComparison comparison)
            => chars.AsSpan().Compare(other.AsSpan(), comparison);
    }

    extension(scoped text text)
    {
        public int Compare(in char other)
            => text.Compare(other.AsSpan());

        public int Compare(char[]? other)
            => text.Compare(other.AsSpan());

        public int Compare(scoped text other)
        {
            return MemoryExtensions.SequenceCompareTo<char>(text, other);
        }

        public int Compare(string? other)
            => text.Compare(other.AsSpan());


        public int Compare(in char other, StringComparison comparison)
            => text.Compare(other.AsSpan(), comparison);

        public int Compare(char[]? other, StringComparison comparison)
            => text.Compare(other.AsSpan(), comparison);

        public int Compare(scoped text other, StringComparison comparison)
        {
            return MemoryExtensions.CompareTo(text, other, comparison);
        }

        public int Compare(string? other, StringComparison comparison)
            => text.Compare(other.AsSpan(), comparison);
    }

    extension(string? str)
    {
        public int Compare(in char other)
            => str.AsSpan().Compare(other.AsSpan());

        public int Compare(char[]? other)
            => str.AsSpan().Compare(other.AsSpan());

        public int Compare(scoped text other)
            => str.AsSpan().Compare(other);

        public int Compare(string? other)
            => str.AsSpan().Compare(other.AsSpan());

        public int Compare(in char other, StringComparison comparison)
            => str.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(char[]? other, StringComparison comparison)
            => str.AsSpan().Compare(other.AsSpan(), comparison);

        public int Compare(scoped text other, StringComparison comparison)
            => str.AsSpan().Compare(other, comparison);

        public int Compare(string? other, StringComparison comparison)
            => str.AsSpan().Compare(other.AsSpan(), comparison);
    }

    extension<T>(T? value)
    {
        public int Compare(T? other)
        {
            return Comparer<T>.Default.Compare(value!, other!);
        }

        public int Compare(T? other, IComparer<T>? comparer)
        {
            return (comparer ?? Comparer<T>.Default).Compare(value!, other!);
        }
    }

    extension(object? obj)
    {
        public int Compare(object? other)
        {
            return ObjectRelater.Default.Compare(obj, other);
        }

        public int Compare(object? other, IComparer? comparer)
        {
            return (comparer ?? ObjectRelater.Default).Compare(obj, other);
        }
    }
}