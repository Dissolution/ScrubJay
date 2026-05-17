#pragma warning disable CA1710

namespace ScrubJay.Text.Building;

/// <summary>
/// TextBuilder is a fluent string builder (much like <see cref="System.Text.StringBuilder"/>) that rents and returns and underlying
/// <see cref="char"/><see cref="System.Array">[]</see> from <see cref="ArrayPool{T}.Shared"/> by implementing <see cref="System.IDisposable"/>.
/// </summary>
/// <remarks>
/// <see cref="TextBuilder"/> has been designed to avoid throwing Exceptions.<br/>
/// Use of any <see langword="interface"/>-specific methods may still throw; see each Method's XML comments for details.
/// </remarks>
[PublicAPI]
[MustDisposeResource(true)]
public partial class TextBuilder : IDisposable
{
    // character array rented from ArrayPool<char>.Shared
    private char[] _chars;

    // next write position in _chars
    private int _position;

    /// <summary>
    /// Get a <see cref="Span{T}"/> of written <see cref="char">chars</see> in this <see cref="TextBuilder"/>
    /// </summary>
    internal Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.AsSpan(0, _position);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> of the unwritten, available portion of this <see cref="TextBuilder"/>
    /// </summary>
    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.AsSpan(_position);
    }

    /// <summary>
    /// Gets the current capacity for this <see cref="TextBuilder"/> to store characters;<br/>
    /// this will automatically be increased as needed.
    /// </summary>
    internal int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Length;
    }

    /// <summary>
    /// Gets the current count of characters that have been written to this <see cref="TextBuilder"/>
    /// </summary>
    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _position;
        internal set
        {
            Debug.Assert((value >= 0) && (value < Capacity));
            _position = value;
        }
    }

    /// <summary>
    /// Create a new <see cref="TextBuilder"/> instance
    /// </summary>
    [MustDisposeResource(true)]
    public TextBuilder()
    {
        _chars = [];
    }

    /// <summary>
    /// Create a new <see cref="TextBuilder"/> instance with a minimum starting Capacity
    /// </summary>
    /// <param name="minCapacity">
    /// The minimum starting capacity the TextBuilder instance will have (it may be higher)
    /// </param>
    [MustDisposeResource(true)]
    public TextBuilder(int minCapacity)
    {
        int capacity = Math.Max(1024, minCapacity);
        _chars = ArrayPool<char>.Shared.Rent(capacity);
    }

    [HandlesResourceDisposal]
    ~TextBuilder() => Dispose();

    public bool TrySlice(int index, out Span<char> span)
    {
        if ((uint)index < (uint)_position)
        {
            span = _chars.AsSpan(index, _position - index);
            return true;
        }
        span = default;
        return false;
    }

    public bool TrySlice(int index, int length, out Span<char> span)
    {
        if ((uint)index + (uint)length <= (uint)_position)
        {
            span = _chars.AsSpan(index, length);
            return true;
        }
        span = default;
        return false;
    }

    public bool TrySlice(Range range, out Span<char> span)
    {
        int start = range.Start.GetOffset(_position);
        int end = range.End.GetOffset(_position);

        if ((uint)start <= (uint)_position &&
            (uint)end <= (uint)_position &&
            start <= end)
        {
            span = _chars.AsSpan(start, end - start);
            return true;
        }

        span = default;
        return false;
    }

    public bool TryCopyTo(Span<char> destination)
    {
        if (_position <= destination.Length)
        {
            TextHelper.Unsafe.CopyTo(_chars, destination, _position);
            return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _chars.AsSpan(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public text AsText() => new text(_chars, 0, _position);

    public char[] ToArray()
    {
        int len = _position;
        char[] array = new char[len];
        TextHelper.Unsafe.CopyTo(_chars, array, len);
        return array;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TextBuilder textBuilder)
            return Equals((text)textBuilder.Written);
        if (obj is string str)
            return Equals((text)str);
        if (obj is char[] chars)
            return Equals((text)chars);
        return false;
    }

    public bool Equals(scoped text text)
    {
        return _chars.AsSpan(0, _position).Equals(text, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Hasher.HashMany<char>(Written);
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        //_whitespace?.Dispose();
        char[] toReturn = _chars;
        _position = 0;
        _chars = [];
        TextPool.Return(toReturn);
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => new(_chars, 0, _position);

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }
}