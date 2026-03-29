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
    [MustDisposeResource(true)]
    public TextBuilder()
    {
        _charArray = [];
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
        _charArray = ArrayPool<char>.Shared.Rent(capacity);
    }

    [HandlesResourceDisposal]
    ~TextBuilder() => Dispose();

    public Option<char> GetAt(Index index)
        => Validate
            .Index(index, _position)
            .Select(i => _charArray[i])
            .AsOption();

    public Option<char> SetAt(Index index, char ch)
    {
        return Validate.Index(index, _position)
            .Select(i => _charArray[i] = ch)
            .AsOption();
    }


    public Span<char> Slice(int index)
    {
        Guard.Index(index, _position);
        return _charArray.AsSpan(index.._position);
    }

    public Span<char> Slice(Index index)
    {
        int offset = Guard.Index(index, _position);
        return _charArray.AsSpan(offset.._position);
    }

    public Span<char> Slice(int index, int count)
    {
        Guard.Range(index, count, _position);
        return _charArray.AsSpan(index, count);
    }

    public Span<char> Slice(Index index, int count)
    {
        (int offset, int len) = Guard.Range(index, count, _position);
        return _charArray.AsSpan(offset, len);
    }

    public Span<char> Slice(Range range)
    {
        (int offset, int len) = Guard.Range(range, _position);
        return _charArray.AsSpan(offset, len);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _charArray.AsSpan(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public text AsText() => new text(_charArray, 0, _position);

    public char[] ToArray() => _charArray.SubArray(0, _position);

    public Result<int> TryCopyTo(Span<char> destination)
    {
        int len = _position;
        if (Validate.CanCopyTo(len, destination).IsError(out var error))
            return error;
        TextHelper.Notsafe.CopyBlock(_charArray, destination, len);
        return Ok(len);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            string str => Written.Equate(str),
            char[] chars => Written.Equate(chars),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return Hasher.HashMany(Written);
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        _position = 0;
        _whitespace?.Dispose();
        char[] toReturn = Reference.Exchange(ref _charArray, []);
        if (toReturn.Length > 0)
        {
            ArrayPool<char>.Shared.Return(toReturn, true);
        }

        GC.SuppressFinalize(this);
    }
//
//    public TextBuilder RenderTo(TextBuilder builder)
//    {
//        return builder.Append(Written);
//    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        text format = default,
        IFormatProvider? provider = default)
    {
        int len = _position;
        if (len <= destination.Length)
        {
            TextHelper.Notsafe.CopyBlock(_charArray, destination, len);
            charsWritten = len;
            return true;
        }
        else
        {
            charsWritten = 0;
            return false;
        }
    }

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        return Written.AsString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Written.AsString();

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = ToString();
        Dispose();
        return str;
    }
}