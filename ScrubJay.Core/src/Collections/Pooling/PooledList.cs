// Identifiers should have correct suffix

using System.Buffers;
using System.Text;


#pragma warning disable CA1710

namespace ScrubJay.Collections.Pooling;

/// <summary>
/// A PooledList is an <see cref="IList{T}"/> implementation that operates on
/// <see cref="Array">T[]</see> instance and <see cref="ArrayPool{T}"/> to minimize allocations<br/>
/// It must be Disposed in order to be useful
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of items stored in this <see cref="PooledList{T}"/>
/// </typeparam>
[PublicAPI]
[MustDisposeResource]
public sealed class PooledList<T> :
    IList<T>,
    IReadOnlyList<T>,
    ICollection<T>,
    IReadOnlyCollection<T>,
    IEnumerable<T>,
    IDisposable
{
    internal T[] _array;
    internal int _position;
    internal int _version;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    bool ICollection<T>.IsReadOnly => false;

    /// <summary>
    /// Get a <see cref="Span{T}"/> over items in this <see cref="PooledList{T}"/>
    /// </summary>
    internal Span<T> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.AsSpan(0, _position);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> over the unwritten, available portion of this <see cref="PooledList{T}"/>
    /// </summary>
    internal Span<T> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.AsSpan(_position);
    }

    /// <inheritdoc cref="IReadOnlyList{T}.this"/>
    T IReadOnlyList<T>.this[int index] => this[index];

    /// <inheritdoc cref="IList{T}.this"/>
    T IList<T>.this[int index]
    {
        get => this[index];
        set
        {
            _version++;
            this[index] = value;
        }
    }

    /// <summary>
    /// Returns a reference to the item in this <see cref="PooledList{T}"/> at the given <paramref name="index"/>
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when <paramref name="index"/> is invalid
    /// </exception>
    public ref T this[int index]
    {
        get
        {
            // Have to assume changes
            _version++;
            return ref Written[index];
        }
    }

    /// <summary>
    /// Returns a reference to the item in this <see cref="PooledList{T}"/> at the given <see cref="Index"/>
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when <paramref name="index"/> is invalid
    /// </exception>
    public ref T this[Index index]
    {
        get
        {
            // Have to assume changes
            _version++;
            return ref Written[index];
        }
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> over the given <see cref="Range"/> of items in this <see cref="PooledList{T}"/>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="range"/> is invalid
    /// </exception>
    public Span<T> this[Range range]
    {
        get
        {
            // Have to assume changes
            _version++;
            return Written[range];
        }
    }

    /// <summary>
    /// Gets the number of items in this <see cref="PooledList{T}"/>
    /// </summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _position;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal set
        {
            Debug.Assert((value >= 0) && (value < Capacity));
            _version++;
            _position = value;
        }
    }

    /// <summary>
    /// Gets the current capacity to store items<br/>
    /// This will be automatically increased if required or by calling <see cref="Grow"/>
    /// </summary>
    public int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.Length;
    }

    internal PooledList(T[] array, int position)
    {
        _array = array;
        _position = position;
    }

    /// <summary>
    /// Create an unallocated, empty <see cref="PooledList{T}"/>
    /// </summary>
    public PooledList() : base()
    {
        _array = [];
        _position = 0;
        _version = 0;
    }

    /// <summary>
    /// Create a new, empty <see cref="PooledList{T}"/> with at least a starting Capacity of <paramref name="minCapacity"/>
    /// </summary>
    /// <param name="minCapacity">
    /// The minimum starting Capacity this <see cref="PooledList{T}"/> will have
    /// </param>
    /// <remarks>
    /// If <paramref name="minCapacity"/> is greater than 0, an array will be rented from <see cref="ArrayPool{T}"/>
    /// </remarks>
    public PooledList(int minCapacity) : base()
    {
        _array = ArrayNest<T>.Rent(minCapacity);
        _position = 0;
        _version = 0;
    }

    [HandlesResourceDisposal]
    ~PooledList()
    {
        Dispose();
    }

#region Grow

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grow() => GrowTo(Capacity * 2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GrowBy(int adding)
    {
        Debug.Assert(adding > 0);
        GrowTo(Capacity + (adding * 2));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void GrowTo(int minCapacity)
    {
        if (minCapacity > Capacity)
        {
            T[] newArray = ArrayNest<T>.Rent(minCapacity * 2);
            if (_position > 0)
            {
                Written.CopyTo(newArray);
            }

            ArrayNest.Return(_array);
            _array = newArray;
        }
    }

#endregion /Grow

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void InsertManyEnumerable(int index, IEnumerable<T> items)
    {
        // Slow path, fill another buffer and then insert known
        using var buffer = new Buffer<T>();
        buffer.AddMany(items);
        TryInsertMany(index, buffer.Written).ThrowIfError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SliceEqual(Span<T> left, ReadOnlySpan<T> right, int count, IEqualityComparer<T> itemComparer)
    {
        Debug.Assert(left.Length >= count);
        Debug.Assert(right.Length >= count);
        for (int i = 0; i < count; i++)
        {
            if (!itemComparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

#region Add, AddMany

    /// <summary>
    /// Add a new <paramref name="item"/> to this <see cref="PooledList{T}"/>
    /// </summary>
    public void Add(T item)
    {
        _version++;
        int pos = _position;
        if (pos >= Capacity)
        {
            GrowBy(1);
        }

        _array[pos] = item;
        _position = pos + 1;
    }

    /// <summary>
    /// Adds the given <paramref name="items"/> to this <see cref="PooledList{T}"/>
    /// </summary>
    public void AddMany(params ReadOnlySpan<T> items)
    {
        if (!items.IsEmpty)
        {
            _version++;

            int pos = _position;
            int newPos = pos + items.Length;
            if (newPos >= Capacity)
            {
                GrowBy(items.Length);
            }

            Sequence.CopyTo(items, _array.AsSpan(pos));
            _position = newPos;
        }
    }

    /// <summary>
    /// Adds the given <paramref name="items"/> to this <see cref="PooledList{T}"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddMany(T[]? items) => AddMany(items.AsSpan());

    /// <summary>
    /// Adds the given <paramref name="items"/> to this <see cref="PooledList{T}"/>
    /// </summary>
    public void AddMany(IEnumerable<T>? items)
    {
        if (items is null)
            return;

        int itemCount;

#if !NETFRAMEWORK && !NETSTANDARD
        if (items is List<T> list)
        {
            var span = CollectionsMarshal.AsSpan(list);
            AddMany(span);
            return;
        }
#endif

        if (items is ICollection<T> collection)
        {
            itemCount = collection.Count;
            if (itemCount == 0)
                return;

            _version++;
            int pos = _position;
            int newPos = pos + itemCount;
            if (newPos > Capacity)
            {
                GrowBy(itemCount);
            }

            collection.CopyTo(_array, pos);
            _position = newPos;
            return;
        }

        // slow path
        foreach (var item in items)
        {
            Add(item);
        }
    }

#endregion

#region Insert, InsertMany

    /// <inheritdoc cref="IList{T}.Insert"/>
    void IList<T>.Insert(int index, T item) => TryInsert(index, item).OkOrThrow();

    /// <summary>
    /// Try to insert an <paramref name="item"/> into this <see cref="Buffer{T}"/> at <paramref name="index"/>
    /// </summary>
    /// <param name="index">The <see cref="Index"/> to insert the <paramref name="item"/></param>
    /// <param name="item">The item to insert</param>
    /// <returns>
    /// A <see cref="Result{O,E}"/> that contains the <c>int</c> offset the item was inserted at or an <see cref="Exception"/> that describes why insertion failed
    /// </returns>
    public Result<int> TryInsert(Index index, T item)
    {
        int pos = _position;
        if (Validate.InsertIndex(index, pos).IsError(out var error, out var offset))
            return error;

        if (offset == pos)
        {
            Add(item);
            return Ok(offset);
        }

        int newPos = pos + 1;
        if (newPos >= Capacity)
        {
            GrowBy(1);
        }

        _version++;
        Sequence.SelfCopy(_array, offset..pos, (offset + 1)..);
        _array[offset] = item;
        _position = newPos;
        return Ok(offset);
    }

    /// <summary>
    /// Try to insert multiple <paramref name="items"/> into this <see cref="Buffer{T}"/> at <paramref name="index"/>
    /// </summary>
    /// <param name="index">The <see cref="Index"/> to insert the <paramref name="items"/></param>
    /// <param name="items">The <see cref="ReadOnlySpan{T}"/> of items to insert</param>
    /// <returns>
    /// A <see cref="Result{O,E}"/> that contains the <c>int</c> offset the items were inserted at or an <see cref="Exception"/> that describes why insertion failed
    /// </returns>
    public Result<int> TryInsertMany(Index index, scoped ReadOnlySpan<T> items)
    {
        int itemCount = items.Length;

        if (itemCount == 1)
            return TryInsert(index, items[0]);

        if (!Validate.InsertIndex(index, _position).IsOk(out int offset, out var ex))
            return ex;

        if (offset == _position)
        {
            AddMany(items);
            return Ok(offset);
        }

        int newPos = _position + itemCount;
        if (newPos >= Capacity)
        {
            GrowBy(itemCount);
        }

        _version++;
        Sequence.SelfCopy(_array, offset.._position, (offset + itemCount)..);
        Sequence.CopyTo(items, _array.AsSpan(offset, itemCount));
        _position = newPos;
        return Ok(offset);
    }

    /// <summary>
    /// Try to insert multiple <paramref name="items"/> into this <see cref="Buffer{T}"/> at <paramref name="index"/>
    /// </summary>
    /// <param name="index">The <see cref="Index"/> to insert the <paramref name="items"/></param>
    /// <param name="items">The <see cref="Array">T[]</see> of items to insert</param>
    /// <returns>
    /// A <see cref="Result{O,E}"/> that contains the <c>int</c> offset the items were inserted at or an <see cref="Exception"/> that describes why insertion failed
    /// </returns>
    public void TryInsertMany(Index index, params T[]? items) => TryInsertMany(index, new ReadOnlySpan<T>(items));


    /// <summary>
    /// Try to insert multiple <paramref name="items"/> into this <see cref="Buffer{T}"/> at <paramref name="index"/>
    /// </summary>
    /// <param name="index">The <see cref="Index"/> to insert the <paramref name="items"/></param>
    /// <param name="items">The <see cref="IEnumerable{T}"/> of items to insert</param>
    /// <returns>
    /// A <see cref="Result{O,E}"/> that contains the <c>int</c> offset the items were inserted at or an <see cref="Exception"/> that describes why insertion failed
    /// </returns>
    public Result<int> TryInsertMany(Index index, IEnumerable<T>? items)
    {
        int offset;
        Exception? error;

        if (items is null)
        {
            if (Validate.InsertIndex(index, _position).IsOk(out offset, out error))
                return 0;
            return error;
        }

        int pos = _position;

        if (!Validate.InsertIndex(index, pos).IsOk(out offset, out error))
            return error;

        if (offset == _position)
        {
            AddMany(items);
            return Ok(offset);
        }

        int itemCount;
        if (items is ICollection<T> collection)
        {
            itemCount = collection.Count;
            if (itemCount == 0)
                return Ok(offset);

            int newPos = pos + itemCount;
            if (newPos > Capacity)
            {
                GrowBy(itemCount);
            }

            _version++;
            Sequence.SelfCopy(_array, offset.._position, (offset + itemCount)..);
            collection.CopyTo(_array, offset);
            _position = newPos;
            return Ok(offset);
        }

        // Enumerate to a temporary PooledList, then insert
        InsertManyEnumerable(offset, items);
        return Ok(offset);
    }

#endregion /Insert


    /// <summary>
    /// Sorts the items in this <see cref="PooledList{T}"/> using an optional <see cref="IComparer{T}"/>
    /// </summary>
    /// <param name="itemComparer">
    /// An optional <see cref="IComparer{T}"/> used to sort the items, defaults to <see cref="Comparer{T}"/>.<see cref="Comparer{T}.Default"/>
    /// </param>
    public void Sort(IComparer<T>? itemComparer = null)
    {
        _version++;
        Array.Sort(_array, 0, _position, itemComparer);
    }

    /// <summary>
    /// Sorts the items in this <see cref="PooledList{T}"/> using a <see cref="Comparison{T}"/> delegate
    /// </summary>
    /// <param name="itemComparison">
    /// The <see cref="Comparison{T}"/> delegate used to sort the items
    /// </param>
    public void Sort(Comparison<T> itemComparison)
    {
        _version++;
        Array.Sort(_array, 0, _position, Compare.CreateComparer<T>(itemComparison));
    }

    /// <summary>
    /// Reverses the order of the items in this <see cref="PooledList{T}"/>
    /// </summary>
    public void Reverse()
    {
        _version++;
        Array.Reverse(_array);
    }

    /// <summary>
    /// Does this <see cref="PooledList{T}"/> contain any instances of the <paramref name="item"/>?
    /// </summary>
    /// <param name="item">
    /// The item to scan for using <see cref="EqualityComparer{T}"/>.<see cref="EqualityComparer{T}.Default"/>
    /// </param>
    /// <returns>
    /// <c>true</c> if this buffer contains an instance of <paramref name="item"/><br/>
    /// <c>false</c> if it does not
    /// </returns>
    public bool Contains(T item)
    {
        var array = _array;
        for (int i = 0; i < _position; i++)
        {
            if (EqualityComparer<T>.Default.Equals(item, array[i]))
                return true;
        }

        return false;
    }

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    int IList<T>.IndexOf(T item) => TryFindIndex(item).SomeOr(-1);

    /// <summary>
    /// Try to find an <paramref name="item"/> in this <see cref="PooledList{T}"/>
    /// </summary>
    /// <param name="item">The item to search for</param>
    /// <param name="firstToLast">
    /// <c>true</c>: Search from low to high indices<br/>
    /// <c>false</c>: Search from high to low indices<br/>
    /// </param>
    /// <param name="offset">
    /// The <see cref="Index"/> offset in this PooledList to start the search from
    /// </param>
    /// <param name="itemComparer">
    /// An optional <see cref="IEqualityComparer{T}"/> to use for <paramref name="item"/> comparison instead of
    /// <see cref="EqualityComparer{T}"/>.<see cref="EqualityComparer{T}.Default"/>
    /// </param>
    /// <returns>
    /// An <see cref="Option{T}"/> that might contain the index of the first matching instance
    /// </returns>
    public Option<int> TryFindIndex(T item, bool firstToLast = true, Index? offset = null,
        IEqualityComparer<T>? itemComparer = null)
    {
        int pos = _position;
        var span = new Span<T>(_array);

        // get a valid item comparer
        itemComparer ??= EqualityComparer<T>.Default;

        if (firstToLast)
        {
            int index;
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;
            }
            else
            {
                // No offset, we start at the first item
                index = 0;
            }

            // we can scan until the last item
            for (; index < pos; index++)
            {
                if (itemComparer.Equals(span[index], item))
                {
                    return Some(index);
                }
            }
        }
        else // lastToFirst
        {
            int index;
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;
            }
            else
            {
                // No offset, we start at the last item
                index = pos - 1;
            }

            // we can scan until the first item
            for (; index >= 0; index--)
            {
                if (itemComparer.Equals(span[index], item))
                {
                    return Some(index);
                }
            }
        }

        return None;
    }

    /// <summary>
    /// Try to find a sequence of <paramref name="items"/> in this <see cref="Buffer{T}"/>
    /// </summary>
    /// <param name="items">The <see cref="ReadOnlySpan{T}"/> to search for</param>
    /// <param name="firstToLast">
    /// <c>true</c>: Search from low to high indices<br/>
    /// <c>false</c>: Search from high to low indices<br/>
    /// </param>
    /// <param name="offset">
    /// The <see cref="Index"/> offset in this <see cref="Buffer{T}"/> to start the search from
    /// </param>
    /// <param name="itemComparer">
    /// An optional <see cref="IEqualityComparer{T}"/> to use for item comparison instead of
    /// <see cref="EqualityComparer{T}"/>.<see cref="EqualityComparer{T}.Default"/>
    /// </param>
    /// <returns>
    /// An <see cref="Option{T}"/> that might contain the index of the first matching sequence
    /// </returns>
    public Option<int> TryFindIndex(
        ReadOnlySpan<T> items,
        bool firstToLast = true,
        Index? offset = null,
        IEqualityComparer<T>? itemComparer = null)
    {
        int itemCount = items.Length;
        int pos = _position;
        var span = new Span<T>(_array);

        if ((itemCount == 0) || (itemCount > pos))
            return None;

        // we can only scan until an end item (past that there wouldn't be enough items to match)
        int end = pos - itemCount;

        // get a valid item comparer
        itemComparer ??= EqualityComparer<T>.Default;

        if (firstToLast)
        {
            int index;
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;
            }
            else
            {
                // No offset, we start at the first item
                index = 0;
            }

            for (; index <= end; index++)
            {
                if (SliceEqual(span.Slice(index), items, itemCount, itemComparer))
                    return Some(index);
            }
        }
        else // lastToFirst
        {
            int index;
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;

                // No point in scanning until the last valid index
                if (index > end)
                    index = end;
            }
            else
            {
                // No offset, we start at the last valid item
                index = end;
            }

            // we can scan until the first item
            for (; index >= 0; index--)
            {
                if (SliceEqual(span.Slice(index), items, itemCount, itemComparer))
                    return Some(index);
            }
        }

        return None;
    }

    /// <summary>
    /// Try to find the Index and Item that match an <paramref name="itemPredicate"/>
    /// </summary>
    /// <param name="itemPredicate">
    /// The <see cref="Predicate{T}"/> used to determine if a valid item has been found
    /// </param>
    /// <param name="firstToLast"><b>default: true</b><br/>
    /// <c>true</c>: Search from low to high indices<br/>
    /// <c>false</c>: Search from high to low indices
    /// </param>
    /// <param name="offset">
    /// The <see cref="Index"/> offset to start the search from
    /// </param>
    /// <returns>
    /// An <see cref="Option{T}"/> that might contain the first matching Index + Item
    /// </returns>
    public Option<(int Index, T Item)> TryFindItemIndex(
        Func<T, bool>? itemPredicate,
        bool firstToLast = true,
        Index? offset = null)
    {
        if (itemPredicate is null)
            return None;

        int pos = _position;
        var span = new Span<T>(_array);

        int index;
        T item;

        if (firstToLast)
        {
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;
            }
            else
            {
                // No offset, we start at the first item
                index = 0;
            }

            // we can scan until the last item
            for (; index < pos; index++)
            {
                item = span[index];
                if (itemPredicate(item))
                {
                    return Some((index, item));
                }
            }
        }
        else // lastToFirst
        {
            // Check for a starting offset
            if (offset.TryGetValue(out Index offsetIndex))
            {
                // Validate that offset
                var validIndex = Validate.Index(offsetIndex, pos);
                if (!validIndex.IsOk(out index))
                    return None;
            }
            else
            {
                // No offset, we start at the last item
                index = pos - 1;
            }

            // we can scan until the first item
            for (; index >= 0; index--)
            {
                item = span[index];
                if (itemPredicate(span[index]))
                {
                    return Some((index, item));
                }
            }
        }

        return None;
    }


    public Result<T> TryGetAt(Index index)
    {
        if (!Validate.Index(index, _position).IsOk(out var offset, out var error))
            return error;
        return _array[offset];
    }

    public Result<T[]> TryGetMany(Range range)
    {
        if (!Validate.Range(range, _position).IsOk(out var offset, out var length, out var error))
            return error;
        return _array.Slice(offset, length);
    }


    public Result TryGetManyTo(Range range, Span<T> destination)
    {
        if (!Validate.Range(range, _position).IsOk(out var offset, out var length, out var error))
            return error;
        if (length > destination.Length)
            return Ex.Arg(destination, $"Destination span cannot hold {length} items");
        Sequence.CopyTo(_array.AsSpan(offset, length), destination);
        return true;
    }

    public Result TrySetAt(Index index, T item)
    {
        if (!Validate.Index(index, _position).IsOk(out var offset, out var error))
            return error;
        _version++;
        _array[offset] = item;
        return true;
    }

    public Result TrySetMany(Range range, scoped ReadOnlySpan<T> items)
    {
        if (!Validate.Range(range, _position).IsOk(out var offset, out var length, out var error))
            return error;
        if (length > items.Length)
            return new ArgumentException($"{items.Length} items cannot fit in a Range Length of {length}");
        _version++;
        Sequence.CopyTo(items, _array.AsSpan(offset, length));
        return true;
    }

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    void IList<T>.RemoveAt(int index) => TryRemoveAt(index);

    /// <summary>
    /// Try to remove the item at the given <see cref="Index"/>
    /// </summary>
    /// <param name="index">
    /// The <see cref="Index"/> of the item to remove
    /// </param>
    /// <returns>
    /// <c>true</c> if the item was removed<br/>
    /// <c>false</c> if it was not
    /// </returns>
    public Result<int> TryRemoveAt(Index index)
    {
        if (!Validate.Index(index, _position).IsOk(out var offset, out var error))
            return error;
        _version++;
        Sequence.SelfCopy(Written, (offset + 1).., offset..);
        _position--;
        return Ok(offset);
    }


    /// <summary>
    /// Try to remove and return the item at the given <see cref="Index"/>
    /// </summary>
    /// <param name="index">
    /// The <see cref="Index"/> of the item to remove
    /// </param>
    /// <returns>
    /// An <see cref="Option{T}"/> that contains the removed value
    /// </returns>
    public Result<T> TryRemoveAndGetAt(Index index)
    {
        if (!Validate.Index(index, _position).IsOk(out var offset, out var error))
            return error;
        T item = Written[offset];
        _version++;
        Sequence.SelfCopy(Written, (offset + 1).., offset..);
        _position--;
        return Ok(item);
    }

    /// <summary>
    /// Try to remove the items at the given <see cref="Range"/>
    /// </summary>
    /// <param name="range">
    /// The <see cref="Range"/> of items to remove
    /// </param>
    /// <returns>
    /// <c>true</c> if the range of items was removed<br/>
    /// <c>false</c> if they were not
    /// </returns>
    public Result<int> TryRemoveMany(Range range)
    {
        if (!Validate.Range(range, _position).IsOk(out var offset, out var length, out var error))
            return error;
        _version++;
        Sequence.SelfCopy(Written, (offset + length).., offset..);
        _position -= length;
        return Ok(length);
    }

    /// <summary>
    /// Try to remove and return the items at the given <see cref="Range"/>
    /// </summary>
    /// <param name="range">
    /// The <see cref="Range"/> of items to remove
    /// </param>
    /// <returns>
    /// An <see cref="Option{T}"/> containing an <see cref="Array">T[]</see> of removed items
    /// </returns>
    public Result<T[]> TryRemoveAndGetMany(Range range)
    {
        if (!Validate.Range(range, _position).IsOk(out var offset, out var length, out var error))
            return error;
        T[] items = _array.AsSpan(offset, length).ToArray();
        _version++;
        Sequence.SelfCopy(_array, (offset + length).., offset..);
        _position -= length;
        return Ok(items);
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    bool ICollection<T>.Remove(T item) => TryFindIndex(item).IsSomeAnd(index => TryRemoveAt(index));

    /// <summary>
    /// Remove all the items in this <see cref="PooledList{T}"/> that match an <paramref name="itemPredicate"/>
    /// </summary>
    /// <param name="itemPredicate">
    /// The <see cref="Predicate{T}"/> to determine if an item is to be removed
    /// </param>
    /// <returns>
    /// The total number of items removed
    /// </returns>
    public int RemoveWhere(Func<T, bool>? itemPredicate)
    {
        if (itemPredicate is null)
            return 0;

        int freeIndex = 0; // the first free slot in items array
        int pos = _position;
        var array = _array;

        // Find the first item which needs to be removed.
        while ((freeIndex < pos) && !itemPredicate(array[freeIndex]))
            freeIndex++;

        if (freeIndex >= pos)
            return 0;

        int current = freeIndex + 1;
        while (current < pos)
        {
            // Find the first item which needs to be kept.
            while ((current < pos) && itemPredicate(array[current]))
                current++;

            if (current < pos)
            {
                // copy item to the free slot
                _version++;
                array[freeIndex++] = array[current++];
            }
        }

        int removedCount = pos - freeIndex;
        _position = freeIndex;
        return removedCount;
    }

    /// <summary>
    /// Removes all items in this <see cref="PooledList{T}"/>, setting its <see cref="Count"/> to zero
    /// </summary>
    /// <remarks>
    /// This does not release references to any items that had been added, use <see cref="PooledArray{T}.Dispose"/> to ensure proper cleanup
    /// </remarks>
    public void Clear()
    {
        _version++;
        _position = 0;
    }

    /// <summary>
    /// Allocates a <see cref="Span{T}"/> of the given <paramref name="length"/>
    /// </summary>
    /// <param name="length">
    /// The total number of items to allocate a <see cref="Span{T}"/> for
    /// </param>
    /// <returns>
    /// A <see cref="Span{T}"/> over the allocated items
    /// </returns>
    public Span<T> Allocate(int length)
    {
        if (length <= 0)
            return [];

        int pos = _position;
        int newPos = pos + length;
        if (newPos > Capacity)
        {
            GrowBy(length);
        }

        _version++;
        _position = newPos;
        return _array.AsSpan(pos, length);
    }

    public bool TryUseAvailable(FnSpan<T, int> useAvailable)
    {
        int used = useAvailable(Available);
        if ((used < 0) || (used > Available.Length))
            return false;
        _version++;
        _position += used;
        return true;
    }

    /// <summary>
    /// Performs a <see cref="ActRef{T}"/> operation on each item in this <see cref="PooledList{T}"/>
    /// </summary>
    /// <param name="perItem">
    /// The <see cref="ActRef{T}"/> delegate that can mutate items
    /// </param>
    public void ForEach(ActRef<T> perItem)
    {
        int pos = _position;
        var array = _array;
        for (int i = 0; i < pos; i++)
        {
            _version++;
            perItem(ref array[i]);
        }
    }

    /// <summary>
    /// Performs a <see cref="ActRef{T,I}"/> operation on each item in this <see cref="PooledList{T}"/>
    /// </summary>
    /// <param name="perItem">
    /// The <see cref="ActRef{T,I}"/> delegate that can mutate items
    /// </param>
    public void ForEach(ActRef<T, int> perItem)
    {
        int pos = _position;
        var array = _array;
        for (int i = 0; i < pos; i++)
        {
            _version++;
            perItem(ref array[i], i);
        }
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        Guard.CanCopyTo(_position, array, arrayIndex);
        Sequence.CopyTo(Written, array.AsSpan(arrayIndex));
    }

    /// <summary>
    /// Try to copy the items in this <see cref="PooledList{T}"/> to a <see cref="Span{T}"/>
    /// </summary>
    public bool TryCopyTo(Span<T> span) => Written.TryCopyTo(span);

    /// <summary>
    /// Get the <see cref="Span{T}"/> of written items in this <see cref="PooledList{T}"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => _array.AsSpan(0, _position);

    /// <summary>
    /// Gets a <see cref="Span{T}"/> slice of written items starting at the given <paramref name="index"/>
    /// </summary>
    public Span<T> Slice(int index)
    {
        Guard.Index(index, _position);
        return _array.AsSpan(index.._position);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> slice of written items starting at the given <see cref="Index"/>
    /// </summary>
    public Span<T> Slice(Index index)
    {
        int offset = Guard.Index(index, _position);
        return _array.AsSpan(offset.._position);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> slice of written items from <paramref name="index"/> for <paramref name="count"/>
    /// </summary>
    public Span<T> Slice(int index, int count)
    {
        Guard.Range(index, count, _position);
        return _array.AsSpan(index, count);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> slice of written items from <paramref name="index"/> for <paramref name="count"/>
    /// </summary>
    public Span<T> Slice(Index index, int count)
    {
        (int offset, int len) = Guard.Range(index, count,_position);
        return _array.AsSpan(offset, len);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> slice of written items for a <see cref="Range"/>
    /// </summary>
    public Span<T> Slice(Range range)
    {
        (int offset, int len) = Guard.Range(range,_position);
        return _array.AsSpan(offset, len);
    }

#pragma warning disable CA1002
    /// <summary>
    /// Copy the items in this <see cref="PooledList{T}"/> to a new <c>T[]</c>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray() => _array.Slice(0, _position);

    /// <summary>
    /// Convert this <see cref="PooledList{T}"/> to a <see cref="List{T}"/> containing the same items
    /// </summary>
    public List<T> ToList()
    {
        var list = new List<T>(Capacity);
#if NET8_0_OR_GREATER
        var listSpan = CollectionsMarshal.AsSpan<T>(list);
        Sequence.CopyTo(Written, listSpan);
        CollectionsMarshal.SetCount<T>(list, _position);
#elif NETSTANDARD2_1
        foreach (var item in Written)
        {
            list.Add(item);
        }
#else
        list.AddRange(Written);
#endif
        return list;
    }
#pragma warning restore CA1002

#region Disposal

    [HandlesResourceDisposal]
    public void Dispose()
    {
        _version++;
        _position = 0;
        T[] toReturn = Interlocked.Exchange<T[]>(ref _array, []);
        ArrayNest.Return(toReturn);
        GC.SuppressFinalize(this);
    }

#endregion /Disposal


    public (T[], int) Empty()
    {
        T[] array = Interlocked.Exchange<T[]>(ref _array, []);
        int count = _position;
        _position = 0;
        return (array, count);
    }

    public (T[], int) Replace(T[] array, int count)
    {
        var oldArray = Interlocked.Exchange<T[]>(ref _array, array);
        var oldPos = _position;
        _position = count;
        return (oldArray, oldPos);
    }

    public bool SequenceEqual(ReadOnlySpan<T> items) => Sequence.Equal(Written, items);

    public bool SequenceEqual(params T[]? items) => Sequence.Equal(Written, items);

    public bool SequenceEqual(IEnumerable<T>? items) => Sequence.Equal(Written, items);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null)
            return false;
        if (obj is T[] array)
            return SequenceEqual(array);
        if (obj is IEnumerable<T> enumerable)
            return SequenceEqual(enumerable);
        return false;
    }

    public override int GetHashCode() => Hasher.HashMany<T>(Written);

    /// <summary>
    /// Gets a <see cref="string"/> representation of the <see cref="Written"/> items
    /// </summary>
    public override string ToString()
    {
        var written = Written;
        // Special handling for textual types
        if (typeof(T) == typeof(char))
        {
            return written.ToString(); // will convert directly to a string
        }

        StringBuilder text = new StringBuilder()
            .Append('[');

        if (written.Length > 0)
        {
            text.Append(written[0]);
            for (int i = 1; i < written.Length; i++)
            {
                text.Append(", ");
                text.Append(written[i]);
            }
        }

        text.Append(']');
        return text.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public IEnumerator<T> GetEnumerator()
    {
        if (_position == 0)
            return Enumerators.Empty<T>();
        return new ArrayEnumerator<T>(
            _array,
            0, _position - 1,
            +1,
            () => _version);
    }
}