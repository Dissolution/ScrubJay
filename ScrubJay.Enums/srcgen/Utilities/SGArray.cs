using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ScrubJay.Enums.SourceGen.Coding;

namespace ScrubJay.Enums.SourceGen.Utilities;

public static class SGArray
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SGArray<T> Create<T>(params T[]? array) => new(array);

    public static SGArray<T> Create<T>(ImmutableArray<T> array)
    {
        if (array.IsDefault)
            return SGArray<T>.Default;
        if (array.IsEmpty)
            return SGArray<T>.Empty;
        return new(array.ToArray());
    }

    public static SGArray<N> Create<O, N>(ImmutableArray<O> immutableArray, Func<O, N> converter)
    {
        if (immutableArray.IsDefault)
            return SGArray<N>.Default;
        if (immutableArray.IsEmpty)
            return SGArray<N>.Empty;

        int count = immutableArray.Length;
        N[] newArray = new N[count];
        for (var i = 0; i < count; i++)
        {
            newArray[i] = converter(immutableArray[i]);
        }
        return new(newArray);
    }

    public static SGArray<T> Create<T>(System.Collections.Generic.List<T>? list)
    {
        if (list is not null)
        {
            if (list.Count > 0)
            {
                return new(list.ToArray());
            }
            return SGArray<T>.Empty;
        }
        return SGArray<T>.Default;
    }

    public static SGArray<T> Create<T>(IEnumerable<T>? enumerable)
    {
        if (enumerable is not null)
        {
            if (enumerable is T[] array)
            {
                return new(array);
            }

            if (enumerable is ICollection<T> collection)
            {
                int count = collection.Count;
                if (count == 0)
                    return SGArray<T>.Empty;

                array = new T[count];
                collection.CopyTo(array, 0);
                return new(array);
            }

            return new(enumerable.ToArray());
        }
        return SGArray<T>.Default;
    }
}

public readonly struct SGArray<T> : System.Collections.Generic.IReadOnlyList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEquatable<SGArray<T>>,
    IEquatable<T[]>,
    IFormattable
{
    public static implicit operator SGArray<T>(T[]? array) => SGArray.Create<T>(array);
    public static implicit operator SGArray<T>(ImmutableArray<T> array) => SGArray.Create<T>(array);
    public static implicit operator SGArray<T>(System.Collections.Generic.List<T>? list) => SGArray.Create<T>(list);

    public static bool operator ==(SGArray<T> left, SGArray<T> right) => left.Equals(right);
    public static bool operator !=(SGArray<T> left, SGArray<T> right) => !left.Equals(right);

    public static readonly SGArray<T> Default = new(null);

    public static readonly SGArray<T> Empty = new([]);


    private readonly T[]? _array;

    bool ICollection<T>.IsReadOnly => true;

    public T this[int index]
    {
        get
        {
            if (_array is not null)
                return _array[index];
            throw new InvalidOperationException($"SGArray<{typeof(T).Name}> is not initialized");
        }
    }

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array?.Length ?? 0;
    }

    public bool IsDefault => _array is null;

    public bool IsEmpty => _array is null || _array.Length == 0;

    internal SGArray(T[]? array)
    {
        _array = array;
    }

    void ICollection<T>.Add(T item) => throw new InvalidOperationException($"SGArray<{typeof(T).Name}> is immutable");

    void ICollection<T>.Clear() => throw new InvalidOperationException($"SGArray<{typeof(T).Name}> is immutable");

    bool ICollection<T>.Remove(T item) => throw new InvalidOperationException($"SGArray<{typeof(T).Name}> is immutable");

    public int FirstIndexOf(T item)
    {
        if (_array is not null)
        {
            return Array.IndexOf<T>(_array, item);
        }
        return -1;
    }

    public int LastIndexOf(T item)
    {
        if (_array is not null)
        {
            return Array.LastIndexOf<T>(_array, item);
        }
        return -1;
    }

    public bool Contains(T item) => FirstIndexOf(item) >= 0;

    public void CopyTo(T[] array, int arrayIndex = 0)
    {
        if (_array is null) return;
        int count = _array.Length;

        if ((uint)arrayIndex + (uint)count < (uint)array.Length)
        {
            _array.CopyTo(array.AsSpan(arrayIndex));
            return;
        }
        throw new InvalidOperationException($"Can not copy {count} items to {typeof(T).Name}[{arrayIndex}..{count}]");
    }

    public void CopyTo(Span<T> destination)
    {
        if (_array is not null)
        {
            _array.CopyTo(destination);
        }
    }

    public bool Equals(SGArray<T> other)
    {
        return Equals(other._array);
    }

    public bool Equals(ImmutableArray<T> other)
    {
        var array = _array;

        if (array is null || other.IsDefault)
            return array is null && other.IsDefault;

        int count = array.Length;
        if (other.Length != count)
            return false;

        for (var i = 0; i < count; i++)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(array[i], other[i]))
                return false;
        }

        return true;
    }

    public bool Equals(T[]? other)
    {
        var array = _array;

        if (array is null || other is null)
            return array is null && other is null;

        int count = array.Length;
        if (other.Length != count)
            return false;

        for (var i = 0; i < count; i++)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(array[i], other[i]))
                return false;
        }

        return true;
    }

    public bool Equals(params ReadOnlySpan<T> other)
    {
        var array = _array;

        if (array is null)
            return false;

        int count = array.Length;
        if (other.Length != count)
            return false;

        for (var i = 0; i < count; i++)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(array[i], other[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is SGArray<T> sgArray)
            return Equals(sgArray);
        if (obj is ImmutableArray<T> immutableArray)
            return Equals(immutableArray);
        if (obj is T[] array)
            return Equals(array);
        return false;
    }

    public override int GetHashCode()
    {
        return FNV1a.Hash(_array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsSpan() => new ReadOnlySpan<T>(_array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<T> AsMemory() => new ReadOnlyMemory<T>(_array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[]? AsArray() => _array;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    public EquatableArrayEnumerator GetEnumerator() => new(_array);

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        var array = _array;
        if (array is null)
            return string.Empty;
        int len = array.Length;
        if (len == 0)
            return "[]";

        return new CodeBuilder()
            .Append('[')
            .Delimit(", ", array, (cb, item) => cb.Format<T>(item, format, provider))
            .Append(']')
            .ToStringAndDispose();
    }

    public override string ToString()
    {
        var array = _array;
        if (array is null)
            return string.Empty;
        int len = array.Length;
        if (len == 0)
            return "[]";

        return new CodeBuilder()
            .Append('[')
            .Delimit(", ", array, (cb, item) => cb.Format<T>(item))
            .Append(']')
            .ToStringAndDispose();
    }

    public struct EquatableArrayEnumerator : IEnumerator<T>
    {
        private readonly T[]? _array;
        private int _index;

        readonly object? IEnumerator.Current => Current;

        public readonly T Current
        {
            get
            {
                if (_array is not null && (uint)_index < (uint)_array.Length)
                    return _array[_index];
                throw new InvalidOperationException();
            }
        }

        internal EquatableArrayEnumerator(T[]? array)
        {
            _array = array;
            _index = -1;
        }

        public bool MoveNext()
        {
            if (_array is not null)
            {
                int nextIndex = _index + 1;
                if (nextIndex < _array.Length)
                {
                    _index = nextIndex;
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
        }

        readonly void IDisposable.Dispose()
        {
            // do nothing
        }
    }
}