#pragma warning disable CA1710

using ScrubJay.Text.Pooling;
using ScrubJay.Text.Utilities;

namespace ScrubJay.Text.Building;

/// <summary>
/// TextBuilder is a fluent string builder (much like <see cref="System.Text.StringBuilder"/>) that rents and returns and underlying
/// <see cref="char"/><see cref="Array">[]</see> from <see cref="ArrayPool{T}.Shared"/> by implementing <see cref="IDisposable"/>.
/// </summary>
/// <remarks>
/// This has been designed to cause as few allocations as possible and to have the fewest chances of an exception being thrown.
/// </remarks>
[PublicAPI]
[MustDisposeResource(true)]
public ref partial struct TextBuilder :
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IFormattable,
    IDisposable
{
    private char[]? _charArray;
    private Span<char> _charSpan;
    private int _position;

    /// <summary>
    /// Get a <see cref="Span{T}"/> of written <see cref="char">chars</see> in this <see cref="TextBuilder"/>
    /// </summary>
    internal Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(0, _position);
    }

    /// <summary>
    /// Gets a <see cref="Span{T}"/> of the unwritten, available portion of this <see cref="TextBuilder"/>
    /// </summary>
    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(_position);
    }

    /// <summary>
    /// Gets the current capacity for this <see cref="TextBuilder"/> to store characters;<br/>
    /// this will automatically be increased as needed.
    /// </summary>
    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Length;
    }

    /// <summary>
    /// Gets or sets the count of characters that have been written,<br/>
    /// clamped between 0 and <see cref="Capacity"/>.
    /// </summary>
    /// <remarks>
    /// If you write anything into <see cref="Available"/> directly, you will need to increment this as well.
    /// </remarks>
    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _position;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (value < 0)
            {
                _position = 0;
            }
            else if (value > _charSpan.Length)
            {
                _position = _charSpan.Length;
            }
            else
            {
                _position = value;
            }
        }
    }

    private TextBuilder(Span<char> span, int position)
    {
        _charArray = null;
        _charSpan = span;
        _position = position;
    }

    /// <summary>
    /// Create a new <see cref="TextBuilder"/> instance
    /// </summary>
    public TextBuilder()
    {
        _charArray = null;
        _charSpan = [];
        _position = 0;
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
        _charSpan = _charArray = TextPool.Rent(minCapacity);
        _position = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialBuffer"></param>
    /// <remarks>
    /// Can be used with <see langword="stackalloc"/>.
    /// </remarks>
    public TextBuilder(Span<char> initialBuffer)
    {
        _charArray = null;
        _charSpan = initialBuffer;
        _position = 0;
    }

    public Option<char> GetAt(int index)
    {
        if ((uint)index < (uint)_position)
            return Some(_charSpan[index]);
        return None;
    }


    public Option<char> GetAt(Index index)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset < (uint)_position)
            return Some(_charSpan[offset]);
        return None;
    }

    public Option<char> SetAt(int index, char ch)
    {
        if ((uint)index < (uint)_position)
        {
            _charSpan[index] = ch;
            return Some(ch);
        }
        return None;
    }

    public Option<char> SetAt(Index index, char ch)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset < (uint)_position)
        {
            _charSpan[offset] = ch;
            return Some(ch);
        }
        return None;
    }

    public bool TrySlice(int index, out Span<char> slice)
    {
        if ((uint)index <= (uint)_position)
        {
            slice = _charSpan.Slice(index, _position - index);
            return true;
        }
        slice = default;
        return false;
    }

    public bool TrySlice(Index index, out Span<char> slice)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset <= (uint)_position)
        {
            slice = _charSpan.Slice(offset, _position - offset);
            return true;
        }
        slice = default;
        return false;
    }

    public bool TrySlice(int index, int count, out Span<char> slice)
    {
        if ((uint)index + (uint)count <= (uint)_position)
        {
            slice = _charSpan.Slice(index, count);
            return true;
        }
        slice = default;
        return false;
    }

    public bool TrySlice(Index index, int count, out Span<char> slice)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset + (uint)count <= (uint)_position)
        {
            slice = _charSpan.Slice(offset, count);
            return true;
        }
        slice = default;
        return false;
    }

    public bool TrySlice(Range range, out Span<char> slice)
    {
        int start = range.Start.GetOffset(_position);
        int end = range.End.GetOffset(_position);
        Debug.Assert(start <= _position);
        Debug.Assert(end <= _position);
        if (start >= 0 && start <= end)
        {
            slice = _charSpan.Slice(start, end - start);
            return true;
        }
        slice = default;
        return false;
    }

    public bool TryCopyTo(Span<char> destination)
    {
        if (_position <= destination.Length)
        {
            TextHelper.Unsafe.CopyTo(_charSpan, destination, _position);
            return true;
        }
        return false;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            string str => Written.Equals(str, StringComparison.Ordinal),
            char[] chars => Written.Equals(chars, StringComparison.Ordinal),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return Hasher.HashMany<char>(Written);
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        char[]? toReturn = _charArray;
        this = default;
        TextPool.Return(toReturn);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        text format = default,
        IFormatProvider? provider = default)
    {
        if (_position <= destination.Length)
        {
            TextHelper.Unsafe.CopyTo(_charSpan, destination, _position);
            charsWritten = _position;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? provider = null)
        => ToString();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _charSpan.Slice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly text AsText() => _charSpan.Slice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly char[] ToArray() => _charSpan.Slice(0, _position).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString()
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        => new string(_charSpan.Slice(0, _position));
#else
        => _charSpan.Slice(0, _position).ToString();
#endif

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = ToString();
        Dispose();
        return str;
    }
}