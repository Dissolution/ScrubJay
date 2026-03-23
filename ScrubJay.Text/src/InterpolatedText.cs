using ScrubJay.Text.Pooling;
using ScrubJay.Text.Utilities;
// ReSharper disable MergeCastWithTypeCheck

namespace ScrubJay.Text;

[PublicAPI]
[InterpolatedStringHandler]
[MustDisposeResource(true)]
public ref struct InterpolatedText
{
    // more aggressive that DefaultISH

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetDefaultLength(int literalLength, int formattedCount)
        => literalLength + (formattedCount * 16);


    private char[]? _charArray;
    private Span<char> _charSpan;
    private int _position;

    public Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(0, _position);
    }

    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(_position);
    }

    public readonly int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _position;
    }

    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Length;
    }

    public InterpolatedText(int literalLength, int formattedCount)
    {
        _charSpan = _charArray = TextPool.Rent(GetDefaultLength(literalLength, formattedCount));
        _position = 0;
    }

    public InterpolatedText(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _charSpan = initialBuffer;
        _position = 0;
    }

    public InterpolatedText(Span<char> initialBuffer)
    {
        _charSpan = initialBuffer;
        _position = 0;
    }

#region Grow
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowBy(int additionalChars)
    {
        Debug.Assert(additionalChars > _charSpan.Length - _position);
        char[] newArray = TextPool.Rent((_charSpan.Length + additionalChars) * 2);
        TextHelper.Notsafe.CopyBlock(_charSpan, newArray, _position);
        char[]? toReturn = _charArray;
        _charSpan = _charArray = newArray;
        TextPool.Return(toReturn);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow()
    {
        char[] newArray = TextPool.Rent(_charSpan.Length * 2);
        TextHelper.Notsafe.CopyBlock(_charSpan, newArray, _position);
        char[]? toReturn = _charArray;
        _charSpan = _charArray = newArray;
        TextPool.Return(toReturn);
    }

    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenCopyString(string str)
    {
        int len = str.Length;
        GrowBy(len);
        TextHelper.Notsafe.CopyBlock(str, Available, len);
        _position += len;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenCopySpan(scoped text text)
    {
        int len = text.Length;
        GrowBy(len);
        TextHelper.Notsafe.CopyBlock(text, Available, len);
        _position += len;
    }
#endregion



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string str)
    {
        Debug.Assert(str is not null);
        if (TextHelper.TryCopyTo(str, Available))
        {
            _position += str.Length;
        }
        else
        {
            GrowThenCopyString(str);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(ref readonly char ch)
    {
        if (_position < Capacity)
        {
            _charSpan[_position] = ch;
            _position++;
        }
        else
        {
            GrowThenCopySpan(ch.AsSpan());
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(scoped text text)
    {
        if (TextHelper.TryCopyTo(text, Available))
        {
            _position += text.Length;
        }
        else
        {
            GrowThenCopySpan(text);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(string? str)
    {
        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

    public void AppendFormatted<T>(T value)
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
                // constrained call avoiding boxing for value types
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, default))
                {
                    Grow();
                }

                _position += charsWritten;
                return;
            }
#endif

            // constrained call avoiding boxing for value types
            str = ((IFormattable)value).ToString(default, default);
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

    public void AppendFormatted<T>(T value, string? format)
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
                // constrained call avoiding boxing for value types
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, default))
                {
                    Grow();
                }

                _position += charsWritten;
                return;
            }
#endif
            // constrained call avoiding boxing for value types
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

    /*





    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value, int alignment)
    {
        int startingPos = _position;
        AppendFormatted(value);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format string.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value, int alignment, string? format)
    {
        int startingPos = _position;
        AppendFormatted(value, format);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }



    /// <summary>Writes the specified string of chars to the handler.</summary>
    /// <param name="value">The span to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted(scoped text value, int alignment = 0, string? format = null)
    {
        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingRequired = alignment - value.Length;
        if (paddingRequired <= 0)
        {
            // The value is as large or larger than the required amount of padding,
            // so just write the value.
            AppendFormatted(value);
            return;
        }

        // Write the value along with the appropriate padding.
        EnsureCapacityForAdditionalChars(value.Length + paddingRequired);
        if (leftAlign)
        {
            value.CopyTo(_charSpan.Slice(_position));
            _position += value.Length;
            _charSpan.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
        }
        else
        {
            _charSpan.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
            value.CopyTo(_charSpan.Slice(_position));
            _position += value.Length;
        }
    }

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null) =>
        // Format is meaningless for strings and doesn't make sense for someone to specify.  We have the overload
        // simply to disambiguate between ROS<char> and object, just in case someone does specify a format, as
        // string is implicitly convertible to both. Just delegate to the T-based implementation.
        AppendFormatted<string?>(value, alignment, format);


    /// <summary>Handles adding any padding required for aligning a formatted value in an interpolation expression.</summary>
    /// <param name="startingPos">The position at which the written value started.</param>
    /// <param name="alignment">Non-zero minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    private void AppendOrInsertAlignmentIfNeeded(int startingPos, int alignment)
    {
        Debug.Assert(startingPos >= 0 && startingPos <= _position);
        Debug.Assert(alignment != 0);

        int charsWritten = _position - startingPos;

        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingNeeded = alignment - charsWritten;
        if (paddingNeeded > 0)
        {
            EnsureCapacityForAdditionalChars(paddingNeeded);

            if (leftAlign)
            {
                _charSpan.Slice(_position, paddingNeeded).Fill(' ');
            }
            else
            {
                _charSpan.Slice(startingPos, charsWritten).CopyTo(_charSpan.Slice(startingPos + paddingNeeded));
                _charSpan.Slice(startingPos, paddingNeeded).Fill(' ');
            }

            _position += paddingNeeded;
        }
    }

    /// <summary>Ensures <see cref="_charSpan"/> has the capacity to store <paramref name="additionalChars"/> beyond <see cref="_position"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityForAdditionalChars(int additionalChars)
    {
        if (_charSpan.Length - _position < additionalChars)
        {
            GrowBy(additionalChars);
        }
    }

    */





    [HandlesResourceDisposal]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = _charArray;

        // Defensive clear
        this = default;

        TextPool.Return(toReturn);
    }
    
    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string result = ToString();
        Dispose();
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString() => _charSpan.Slice(0, _position).ToString();
}