#pragma warning disable CA1710

using System.Buffers;
using ScrubJay.Enhancements.Text.Pooling;
using ScrubJay.Enhancements.Text.Utilities;
using ScrubJay.Enhancements.Validation;

namespace ScrubJay.Enhancements.Text.Building;

/// <summary>
/// TextBuilder is a fluent string builder (much like <see cref="System.Text.StringBuilder"/>) that rents and returns and underlying
/// <see cref="char"/><see cref="Array">[]</see> from <see cref="ArrayPool{T}.Shared"/> by implementing <see cref="IDisposable"/>.
/// </summary>
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

    public ref char this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index < _position)
            {
                return ref _charSpan.UnsafeRef(index);
            }
            throw Ex.ArgRange(in index, 0.._position);
        }
    }
    
    /// <summary>
    /// Gets a <c>ref</c> to the written <see cref="char"/> at <paramref name="index"/>
    /// </summary>
    /// <param name="index">
    /// The <see cref="Index"/> of the character to reference
    /// </param>
    public ref char this[Index index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            int offset = index.GetOffset(_position);
            if ((uint)offset < _position)
            {
                return ref _charSpan.UnsafeRef(offset);
            }
            throw Ex.ArgRange(in index, 0.._position);
        }
    }

    /// <summary>
    /// Gets a <see cref="Span{T}">Span&lt;char&gt;</see> over the <paramref name="range"/> of written characters
    /// </summary>
    /// <param name="range">
    /// The <see cref="Range"/> of the characters to reference
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the given <paramref name="range"/> is invalid
    /// </exception>
    public Span<char> this[Range range]
    {
        get
        {
            (int start, int length) = Validate.Range(range, _position).OkOrThrow();
            return _charSpan.UnsafeSlice(start, length);
        }
    }

    /// <summary>
    /// Gets or sets the count of characters that have been written.
    /// </summary>
    /// <remarks>
    /// If you write anything into <see cref="Available"/> directly, you will need to increment this as well.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if you try to set a Length &lt; 0 or &gt; <see cref="Capacity"/>.
    /// </exception>
    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _position;
        set
        {
            if ((uint)value <= Capacity)
            {
                int pos = _position;
                
                if (value > pos)
                {
                    TextHelper.Clear(_charSpan.UnsafeSlice(pos, value - pos));
                }
                _position = value;
            }
            else
            {
                throw Ex.ArgRange(in value, $"[..{Capacity}]");
            }
        }
    }

    /// <summary>
    /// Create a new <see cref="TextBuilder"/> instance
    /// </summary>
    public TextBuilder()
    {
        _charSpan = _charArray = [];
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

    public TextBuilder(Span<char> initialBuffer, int initialPosition = 0)
    {
        _charArray = null;
        _charSpan = initialBuffer;
        if ((uint)initialPosition <= initialBuffer.Length)
        {
            _position = initialPosition;
        }
        else
        {
            throw Ex.ArgRange(in initialPosition, $"[0..{initialBuffer.Length}]");
        }
    }

    public Option<char> GetAt(int index)
    {
        if ((uint)index < (uint)_position)
            return Some(_charSpan.UnsafeRef(index));
        return None;
    }

    
    public Option<char> GetAt(Index index)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset < (uint)_position)
            return Some(_charSpan.UnsafeRef(offset));
        return None;
    }
    
    public Option<char> SetAt(int index, char ch)
    {
        if ((uint)index < (uint)_position)
        {
            _charSpan.UnsafeRef(index) = ch;
            return Some(ch);
        }
        return None;
    }
    
    public Option<char> SetAt(Index index, char ch)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset < (uint)_position)
        {
            _charSpan.UnsafeRef(offset) = ch;
            return Some(ch);
        }
        return None;
    }
    
    public Span<char> Slice(int index)
    {
        if ((uint)index <= (uint)_position)
        {
            return _charSpan.UnsafeSlice(index, _position - index);
        }
        throw Ex.ArgRange(in index, $"[0..{_position}]");
    }

    public Span<char> Slice(Index index)
    {
        int offset = index.GetOffset(_position);
        if ((uint)offset <= (uint)_position)
        {
            return _charSpan.UnsafeSlice(offset, _position - offset);
        }
        throw Ex.ArgRange(in index, $"[0..{_position}]");
    }

    public Span<char> Slice(int index, int count)
    {
        Throw.IfBadRange(index, count, _position);
        return _charSpan.UnsafeSlice(index, count);
    }

    public Span<char> Slice(Index index, int count)
    {
        (int offset, int len) = Validate.Range(index, count, _position).OkOrThrow();
        return _charSpan.UnsafeSlice(offset, len);
    }

    public Span<char> Slice(Range range)
    {
        (int offset, int len) = Validate.Range(range, _position).OkOrThrow();
        return _charSpan.UnsafeSlice(offset, len);
    }
    
    public Result<int> TryCopyTo(Span<char> destination)
    {
        if (_position <= destination.Length)
        {
            _charSpan.UnsafeCopyTo(destination, _position);
            return _position;
        }
        return Ex.Arg(in destination, $"Length of {destination.Length} was smaller than {_position} needed");
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
        return Hasher.Hash(Written);
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
        if (TryCopyTo(destination).IsOk(out int written))
        {
            charsWritten = written;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? provider = null)
        => ToString();

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _charSpan.UnsafeSlice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public text AsText() => _charSpan.UnsafeSlice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char[] ToArray() => AsSpan().ToArray();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString() => _charSpan.UnsafeToString(_position);

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = ToString();
        Dispose();
        return str;
    }
}