using System.Buffers;

namespace ScrubJay.Universal;

/// <summary>
/// An implementation of an InterpolatedStringHandler that can append any generic value.
/// </summary>\
[MustDisposeResource]
[InterpolatedStringHandler]
public ref struct InterpolatedText
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char[] RentArray(int minCapacity)
    {
        minCapacity = Math.Clamp(minCapacity, 256, 0x3FFFFFDF); // string.MaxLength
        return ArrayPool<char>.Shared.Rent(minCapacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReturnArray(char[]? array)
    {
        if (array is not null && array.Length > 0)
        {
            ArrayPool<char>.Shared.Return(array);
        }
    }

    [MustDisposeResource]
    public static ref InterpolatedText Create(ref InterpolatedText text)
    {
        return ref text;
    }
    

    private char[]? _charArray;
    private Span<char> _charSpan;
    private int _position;

    public ReadOnlySpan<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(0, _position);
    }

    public InterpolatedText(int literalLength, int formattedCount)
    {
        _charSpan = _charArray = RentArray(literalLength + (formattedCount * 16));
        _position = 0;
    }

    public InterpolatedText(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _charArray = null;
        _charSpan = initialBuffer;
        _position = 0;
    }

    public InterpolatedText(int minCapacity)
    {
        _charSpan = _charArray = RentArray(minCapacity);
        _position = 0;
    }

    public InterpolatedText(Span<char> initialBuffer)
    {
        _charArray = null;
        _charSpan = initialBuffer;
        _position = 0;
    }

#region Grow
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowImpl(int requiredMinCapacity)
    {
        int newCapacity = Math.Max(requiredMinCapacity, _charSpan.Length * 2);
        var newArray = RentArray(newCapacity);
        Written.CopyTo(newArray);

        char[]? toReturn = _charArray;
        _charSpan = _charArray = newArray;
        ReturnArray(toReturn);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowBy(int adding)
    {
        Debug.Assert(adding > _charSpan.Length - _position);
        GrowImpl(_position + adding);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenAppendString(string value)
    {
        GrowBy(value.Length);
        value.CopyTo(_charSpan.Slice(_position));
        _position += value.Length;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenAppendChar(char ch)
    {
        GrowBy(1);
        _charSpan[_position++] = ch;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenAppendSpan(scoped ReadOnlySpan<char> value)
    {
        GrowBy(value.Length);
        value.CopyTo(_charSpan.Slice(_position));
        _position += value.Length;
    }
#endregion

    public void AppendLiteral(char ch)
    {
        if (_position < _charSpan.Length)
        {
            _charSpan[_position++] = ch;
        }
        else
        {
            GrowThenAppendChar(ch);
        }
    }

    public void AppendLiteral(string str)
    {
        if (str.TryCopyTo(_charSpan.Slice(_position)))
        {
            _position += str.Length;
        }
        else
        {
            GrowThenAppendString(str);
        }
    }

    public void AppendFormatted(char ch)
    {
        if (_position < _charSpan.Length)
        {
            _charSpan[_position++] = ch;
        }
        else
        {
            GrowThenAppendChar(ch);
        }
    }
    
    public void AppendFormatted(string? str)
    {
        if (str is not null)
        {
            if (str.TryCopyTo(_charSpan.Slice(_position)))
            {
                _position += str.Length;
            }
            else
            {
                GrowThenAppendString(str);
            }
        }
    }

    public void AppendFormatted(scoped ReadOnlySpan<char> value)
    {
        // Fast path for when the value fits in the current buffer
        if (value.TryCopyTo(_charSpan.Slice(_position)))
        {
            _position += value.Length;
        }
        else
        {
            GrowThenAppendSpan(value);
        }
    }

    public void AppendFormatted<T>(T? value)
    {
        if (value is null)
        {
            return;
        }

        string? str;
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(_charSpan.Slice(_position), out charsWritten, default, default))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
                return;
            }
#endif

            str = ((IFormattable)value).ToString(null, default);
        }
        else
        {
            str = value.ToString();
        }

        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void AppendFormatted<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        string? str = Any.ToString<T>(in value, _);
        if (str is not null)
        {
            AppendLiteral(str);
        }
    }
#endif

    public void AppendFormatted<T>(T? value, string? format)
    {
        if (value is null)
        {
            return;
        }

        string? str;
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(_charSpan.Slice(_position), out charsWritten, format, default))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
                return;
            }
#endif

            str = ((IFormattable)value).ToString(format, default);
        }
        else
        {
            str = value.ToString();
        }

        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        char[]? toReturn = _charArray;

        // Defensive clear
        this = default;

        ReturnArray(toReturn);
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = ToString();
        Dispose();
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Written.ToString();
}