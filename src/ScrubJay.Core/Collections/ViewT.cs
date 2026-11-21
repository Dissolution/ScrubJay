using System.Collections.Immutable;
using System.ComponentModel;
using ScrubJay.Text;

namespace ScrubJay.Collections;


/// <summary>
/// A view into an array; similar to <see cref="ImmutableArray{T}"/> except this properly supports <c>IEquatable{TSelf}</c>
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of items stored in the view.
/// </typeparam>
[PublicAPI]
[CollectionBuilder(typeof(View), nameof(View.Create))]
[DebuggerDisplay("{Display,nq}")]
public readonly struct View<T> :
    IReadOnlyList<T>,
    IReadOnlyCollection<T>,
    IEnumerable<T>,
    IEqualityOperators<View<T>, View<T>, bool>,
    IEquatable<View<T>>,
    IFormattable
{
#region Operators

    public static implicit operator ReadOnlySpan<T>(View<T> array) => array.AsSpan();

    public static bool operator ==(View<T> left, View<T> right) => left.Equals(right);
    public static bool operator !=(View<T> left, View<T> right) => !left.Equals(right);

    public static bool operator ==(View<T> left, View<T>? right) => left.Equals(right.GetValueOrDefault());
    public static bool operator !=(View<T> left, View<T>? right) => !left.Equals(right.GetValueOrDefault());

    public static bool operator ==(View<T>? left, View<T>? right) => left.GetValueOrDefault().Equals(right.GetValueOrDefault());
    public static bool operator !=(View<T>? left, View<T>? right) => !left.GetValueOrDefault().Equals(right.GetValueOrDefault());

#endregion

    public static readonly View<T> Uninitialized;

    public static readonly View<T> Empty = new View<T>([]);


    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly T[]? _array;

    T IReadOnlyList<T>.this[int index] => this[index];

    /// <summary>
    /// Gets a <see langword="readonly"/> <see langword="ref">reference</see> to the
    /// <typeparamref name="T"/> item at the given <see cref="int"/> <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="NullReferenceException">
    /// Thrown if the <see cref="View{T}"/> is <see cref="Uninitialized"/>.
    /// </exception>
    public ref readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _array![index];
    }

    public ReadOnlySpan<T> this[Range range]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.AsSpan(range);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array!.Length;
    }

    public int Count
    {
        get
        {
            if (_array is not null)
                return _array.Length;

            return 0;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsDefault
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array == null;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array!.Length == 0;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsDefaultOrEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array is null || _array.Length == 0;
    }

    internal View(T[]? items)
    {
        _array = items;
    }

    public bool Any()
    {
        return _array is not null && _array.Length > 0;
    }

    public int FindIndex(Func<T, bool> match, Index? startIndex = null, bool frontToBack = true)
    {
        if (_array is null)
            return -1;

        int start;
        int end;
        int step;
        if (startIndex.HasValue)
        {
            if (frontToBack)
            {
                start = startIndex.GetValueOrDefault().GetOffset(Length);
                end = Length - 1;
                step = 1;
            }
            else
            {
                end = startIndex.GetValueOrDefault().GetOffset(Length);
                start = 0;
                step = -1;
            }
        }
        else
        {
            start = 0;
            end = Length - 1;
            step = frontToBack ? +1 : -1;
        }

        for (int i = start; i <= end; i += step)
        {
            if (match(_array[i]))
                return i;
        }

        return -1;
    }

    public int FirstIndexOf(T item)
    {
        if (_array is not null)
        {
            return Array.IndexOf(_array, item);
        }

        return -1;
    }

    public int LastIndexOf(T item)
    {
        if (_array is not null)
        {
            return Array.LastIndexOf(_array, item);
        }

        return -1;
    }

    public bool Contains(T item) => FirstIndexOf(item) >= 0;

    public bool Contains(T item, IEqualityComparer<T>? itemEqualityComparer)
    {
        if (itemEqualityComparer is null)
            return Contains(item);
        return FindIndex(value => itemEqualityComparer.Equals(value, item)) >= 0;
    }

    public bool TryCopyTo(Span<T> destination)
    {
        if (_array is null)
            return false;

        if (destination.Length >= Length)
        {
            MemoryExtensions.CopyTo(_array, destination);
            return true;
        }

        return false;
    }

    public ReadOnlySpan<T> AsSpan() => new(_array);

    public ReadOnlySpan<T> AsSpan(int start)
    {
        return new ReadOnlySpan<T>(_array, start, Count - start);
    }

    public ReadOnlySpan<T> AsSpan(int start, int length)
    {
        return new ReadOnlySpan<T>(_array, start, length);
    }

    public ReadOnlySpan<T> AsSpan(Range range)
    {
        (int start, int length) = range.GetOffsetAndLength(Length);
        return new ReadOnlySpan<T>(_array, start, length);
    }

    public bool Equals(View<T> view)
    {
        return Equals(view._array);
    }

    public bool Equals(View<T>? view)
    {
        T[]? arr = view.GetValueOrDefault()._array;
        return Equals(arr);
    }

    public bool Equals(T[]? array)
    {
        if (ReferenceEquals(_array, array))
            return true;

        return MemoryExtensions.SequenceEqual<T>(_array, array);
    }

    public bool Equals(ImmutableArray<T> array)
    {
        return MemoryExtensions.SequenceEqual<T>(_array, array.AsSpan());
    }

    public bool Equals(IReadOnlyList<T>? list)
    {
        if (_array is null)
            return list is null;
        if (list is null)
            return false;

        int length = Length;
        if (list.Count != length)
            return false;

        for (int i = 0; i < length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(_array[i], list[i]))
                return false;
        }

        return true;
    }

    public bool Equals(IEnumerable<T>? enumerable)
    {
        if (_array is null)
            return enumerable is null;
        if (enumerable is null)
            return false;

        using var e = enumerable.GetEnumerator();
        for (int i = 0; i < Length; i++)
        {
            if (!e.MoveNext())
                return false;
            if (!EqualityComparer<T>.Default.Equals(_array[i], e.Current))
                return false;
        }

        if (e.MoveNext())
            return false;

        return true;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj switch
        {
            null => _array is null,
            View<T> view => Equals(view),
            T[] array => Equals(array),
            ImmutableArray<T> immutableArray => Equals(immutableArray),
            IReadOnlyList<T> list => Equals(list),
            IEnumerable<T> enumerable => Equals(enumerable),
            _ => false,
        };
    }

    public override int GetHashCode() => HashCode.Combine<T>(_array);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string Display => IsDefault ? "Uninitialized" : $"Length = {Length}";

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        var array = _array;
        if (array is null)
            return "null";

        int len = array.Length;
        Interpolator builder;
        int literalLength = 2 + (2 * (len - 1));
        builder = new(literalLength, len);

        builder.Write('[');
        if (len > 0)
        {
            builder.Format(array[0], format);
            for (int i = 1; i < len; i++)
            {
                builder.Write(", ");
                builder.Format(array[i], format);
            }
        }

        builder.Write(']');
        return builder.ToStringAndDispose();
    }

    public override string ToString()
    {
        var array = _array;
        if (array is null)
            return "null";

        int len = array.Length;
        Interpolator builder = new(2 + (2 * (len - 1)), len);
        builder.Write('[');
        if (len > 0)
        {
            builder.Write(array[0]);
            for (int i = 1; i < len; i++)
            {
                builder.Write(", ");
                builder.Write(array[i]);
            }
        }

        builder.Write(']');
        return builder.ToStringAndDispose();
    }

#region Enumeration

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ViewEnumerator.Create(_array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => ViewEnumerator.Create(_array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ViewIterator GetEnumerator() => ViewIterator.Create(_array);

    [PublicAPI]
    public struct ViewIterator
    {
        private static readonly ViewIterator _empty = new ViewIterator([]);

        public static ViewIterator Create(T[]? array)
        {
            if (array is null || array.Length == 0)
            {
                return _empty;
            }
            else
            {
                return new ViewIterator(array);
            }
        }

        private readonly T[] _array;
        private int _index;

        public T Current => _array[_index];

        private ViewIterator(T[] array)
        {
            _array = array;
            _index = -1;
        }

        public bool MoveNext() => ++_index < _array.Length;
    }

    [MustDisposeResource(false)]
    private sealed class ViewEnumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
        private static readonly IEnumerator<T> _empty = new ViewEnumerator([]);

        public static IEnumerator<T> Create(T[]? array)
        {
            if (array is null || array.Length == 0)
            {
                return _empty;
            }
            else
            {
                return new ViewEnumerator(array);
            }
        }

        private readonly T[] _array;
        private int _index;

        object? IEnumerator.Current => Current;

        public T Current
        {
            get
            {
                if (unchecked((uint)_index) < (uint)_array.Length)
                    return _array[_index];

                // Before first or after last MoveNext.
                throw new InvalidOperationException();
            }
        }

        private ViewEnumerator(T[] array)
        {
            _index = -1;
            _array = array;
        }

        public bool MoveNext()
        {
            int newIndex = _index + 1;
            int length = _array.Length;

            // unsigned math is used to prevent false positive if index + 1 overflows.
            if ((uint)newIndex <= (uint)length)
            {
                _index = newIndex;
                return (uint)newIndex < (uint)length;
            }

            return false;
        }

        public void Reset()
        {
            _index = -1;
        }

        void IDisposable.Dispose()
        {
            // nothing to do here.
        }
    }

#endregion
}