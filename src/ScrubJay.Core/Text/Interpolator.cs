using System.Buffers;
using System.ComponentModel;
using ScrubJay.Errors;

namespace ScrubJay.Text;

[PublicAPI]
[MustDisposeResource(true)]
[InterpolatedStringHandler]
[StructLayout(LayoutKind.Auto)]
public ref struct Interpolator :
    ITextWriter,
    IBufferWriter<char>,
    IDisposable
{
    public static implicit operator Interpolator(Span<char> initialBuffer) => new(initialBuffer);
    public static implicit operator string(Interpolator interpolator) => interpolator.ToString();

    private char[]? _rentedArray;
    private Span<char> _chars;
    private int _position;

    public int Length
    {
        readonly get => _position;
        set => _position = int.Clamp(value, 0, _chars.Length);
    }

    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Length;
    }

    public Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(0, _position);
    }

    public ref char this[int index]
    {
        get
        {
            return ref _chars[Ensure.Index(index, _position)];
        }
    }
    
    public ref char this[Index index]
    {
        get
        {
            return ref _chars[Ensure.Index(index, _position)];
        }
    }

    public Interpolator()
    {
        _chars = _rentedArray = null;
        _position = 0;
    }

    public Interpolator(Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _rentedArray = null;
        _position = 0;
    }

    internal Interpolator(Interpolator interpolator)
    {
        _rentedArray = interpolator._rentedArray;
        _chars = interpolator._chars;
        _position = interpolator._position;
    }

    public Interpolator(int minCapacity)
    {
        _chars = _rentedArray = TextPool.Rent(minCapacity);
        _position = 0;
    }

    public Interpolator(int literalLength, int formattedCount)
    {
        _chars = _rentedArray = TextPool.Rent(literalLength, formattedCount);
        _position = 0;
    }

#pragma warning disable IDE0060
    public Interpolator(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _rentedArray = null;
        _position = 0;
    }

    internal Interpolator(int literalLength, int formattedCount, Interpolator interpolator)
    {
        _rentedArray = interpolator._rentedArray;
        _chars = interpolator._chars;
        _position = interpolator._position;
    }
#pragma warning restore IDE0060

#region Grow

    internal void GrowBy(int additionalChars)
    {
        GrowTo(_position + additionalChars);
    }

    internal void GrowTo(int minCapacity)
    {
        int newCapacity = int.Max(minCapacity, _chars.Length * 2);
        char[] newArray = TextPool.Rent(newCapacity);
        _chars.Slice(0, _position).CopyTo(newArray);

        char[]? toReturn = _rentedArray;
        _chars = _rentedArray = newArray;
        TextPool.Return(toReturn);
    }

#endregion

#region GrowWrite

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenWrite(char ch)
    {
        GrowBy(1);
        _chars[_position] = ch;
        _position++;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenWrite(string str)
    {
        GrowBy(str.Length);
        str.CopyTo(_chars.Slice(_position));
        _position += str.Length;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenWrite(scoped text txt)
    {
        GrowBy(txt.Length);
        txt.CopyTo(_chars.Slice(_position));
        _position += txt.Length;
    }

#endregion

#region Write

    [OverloadResolutionPriority(100)]
    public void Write(char ch)
    {
        if (_position < _chars.Length)
        {
            _chars[_position] = ch;
            _position++;
        }
        else
        {
            GrowThenWrite(ch);
        }
    }

    [OverloadResolutionPriority(90)]
    public void Write(scoped text txt)
    {
        if (txt.TryCopyTo(_chars.Slice(_position)))
        {
            _position += txt.Length;
        }
        else
        {
            GrowThenWrite(txt);
        }
    }

    [OverloadResolutionPriority(80)]
    public void Write(string? str)
    {
        if (str is not null)
        {
            if (str.TryCopyTo(_chars.Slice(_position)))
            {
                _position += str.Length;
            }
            else
            {
                GrowThenWrite(str);
            }
        }
    }

    [OverloadResolutionPriority(70)]
    public void Write<T>(T? value)
    {
        if (value is null)
        {
            return;
        }

        string? str;
        if (value is IFormattable)
        {
            // If the value can format itself directly into our buffer, do so.
            if (value is ISpanFormattable)
            {
                int charsWritten;
                // constrained call avoiding boxing for value types
                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, default, default))
                {
                    GrowBy(1);
                }

                _position += charsWritten;
                return;
            }

            // constrained call avoiding boxing for value types
            str = ((IFormattable)value).ToString(default, default);
        }
        else
        {
            str = value.ToString();
        }

        Write(str);
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(60)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Write<T>(T? value, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => Write(Anything.ToString(value));
#endif

    [OverloadResolutionPriority(95)]
    public void Write(
        [HandlesResourceDisposal]
        [InterpolatedStringHandlerArgument("")] Interpolator interpolatedText)
    {
        // the writing has already happened, we need to take back our variables
        _rentedArray = interpolatedText._rentedArray;
        _chars = interpolatedText._chars;
        _position = interpolatedText._position;
        // do not actually dispose interpolatedText
    }

    void ITextWriter.WriteFormatted<T>(T? value, string? format, IFormatProvider? provider) where T : default => Format<T>(value, format, provider);

#endregion

#region Format

    [OverloadResolutionPriority(100)]
    public void Format<SF>(SF? value, text format, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasISpanFormattable<SF> _ = default)
        where SF : ISpanFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (value is null)
        {
            return;
        }

        int charsWritten;

        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format, provider))
        {
            GrowBy(1);
        }

        _position += charsWritten;
    }

    [OverloadResolutionPriority(90)]
    public void Format<F>(F? value, string? format, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasIFormattable<F> _ = default)
        where F : IFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (value is null)
        {
            return;
        }

        string? str = value.ToString(format, provider);
        Write(str);
    }

    [OverloadResolutionPriority(50)]
    public void Format<T>(T? value, string? format, IFormatProvider? provider = default)
    {
        if (value is null)
        {
            return;
        }

        string? str;
        if (value is IFormattable)
        {
            // If the value can format itself directly into our buffer, do so.
            if (value is ISpanFormattable)
            {
                int charsWritten;
                // constrained call avoiding boxing for value types
                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, format, provider))
                {
                    GrowBy(1);
                }

                _position += charsWritten;
                return;
            }

            // constrained call avoiding boxing for value types
            str = ((IFormattable)value).ToString(format, provider);
        }
        else
        {
            str = value.ToString();
        }

        Write(str);
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(30)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Format<T>(T? value, string? format, IFormatProvider? provider = default, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        string? str = Anything.Format<T>(value, format, provider);
        Write(str);
    }
#endif

#endregion

#region InterpolatedStringHandler Methods

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string str)
    {
        if (str.TryCopyTo(_chars.Slice(_position)))
        {
            _position += str.Length;
        }
        else
        {
            GrowThenWrite(str);
        }
    }

    [OverloadResolutionPriority(100)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(char ch)
        => Write(ch);

    [OverloadResolutionPriority(90)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(text txt)
        => Write(txt);

    [OverloadResolutionPriority(80)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(string? str)
        => Write(str);


    [OverloadResolutionPriority(70)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value)
        => Write<T>(value);

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(60)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void AppendFormatted<T>(T value, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => Write<T>(value);
#endif

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, string? format)
        => Format<T>(value, format);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted([InterpolatedStringHandlerArgument("")] Interpolator interpolatedText)
    {
        // the writing has already happened, we need to take back our variables
        _rentedArray = interpolatedText._rentedArray;
        _chars = interpolatedText._chars;
        _position = interpolatedText._position;
    }

#endregion

#region IBufferWriter<char> methods

    void IBufferWriter<char>.Advance(int count)
    {
        if ((uint)_position + (uint)count > _chars.Length)
            throw new InvalidOperationException($"Cannot advance {count} characters");

        _position += count;
    }

    Memory<char> IBufferWriter<char>.GetMemory(int sizeHint)
    {
        sizeHint = int.Max(sizeHint, 64);
        if (_rentedArray is null || sizeHint + _position > _chars.Length)
        {
            GrowBy(sizeHint);
            Debug.Assert(_rentedArray is not null);
        }

        return new(_rentedArray, _position, _rentedArray.Length - _position);
    }

    Span<char> IBufferWriter<char>.GetSpan(int sizeHint)
    {
        sizeHint = int.Max(sizeHint, 64);
        if (sizeHint + _position > _chars.Length)
        {
            GrowBy(sizeHint);
        }
        return _chars.Slice(_position, sizeHint);
    }

#endregion

    public void TrimLast(char match)
    {
        if (Written.EndsWith(match))
        {
            _position--;
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _chars.Slice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly text AsText() => _chars.Slice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly char[] ToArray() => _chars.Slice(0, _position).ToArray();


    [HandlesResourceDisposal]
    public void Dispose()
    {
        char[]? toReturn = _rentedArray;
        this = default;
        TextPool.Return(toReturn);
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = ToString();
        Dispose();
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString() => new string(_chars.Slice(0, _position));
}