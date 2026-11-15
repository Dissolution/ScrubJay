using System.Buffers;
using System.ComponentModel;

namespace ScrubJay.Text;

[PublicAPI]
[MustDisposeResource(true)]
[InterpolatedStringHandler]
[StructLayout(LayoutKind.Auto)]
public ref struct InterpolatedTextWriter :
    ITextWriter,
    IDisposable,
    IBufferWriter<char>
{
    //[MustDisposeResource]
    public static InterpolatedTextWriter Create(int minCapacity = TextPool.MIN_CAPACITY)
    {
        return new InterpolatedTextWriter(minCapacity);
    }


    private char[]? _rentedArray;
    private Span<char> _chars;
    private int _position;

    public readonly text Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(0, _position);
    }

    public Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(_position);
    }

    public int AvailableLength => _chars.Length - _position;

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _position;
        set => _position = int.Clamp(value, 0, _chars.Length);
    }

    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Length;
    }

    public InterpolatedTextWriter()
    {
        _chars = _rentedArray = null;
        _position = 0;
    }

    public InterpolatedTextWriter(Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _rentedArray = null;
        _position = 0;
    }

    internal InterpolatedTextWriter(InterpolatedTextWriter writer)
    {
        _rentedArray = writer._rentedArray;
        _chars = writer._chars;
        _position = writer._position;
    }

    public InterpolatedTextWriter(int minCapacity)
    {
        _chars = _rentedArray = TextPool.Rent(minCapacity);
        _position = 0;
    }

    public InterpolatedTextWriter(int literalLength, int formattedCount)
    {
        _chars = _rentedArray = TextPool.Rent(literalLength, formattedCount);
        _position = 0;
    }

    public InterpolatedTextWriter(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _rentedArray = null;
        _position = 0;
    }

    internal InterpolatedTextWriter(int literalLength, int formattedCount, InterpolatedTextWriter writer)
    {
        _rentedArray = writer._rentedArray;
        _chars = writer._chars;
        _position = writer._position;
    }

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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(string? str)
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(string? str, int alignment, string? format = null)
        => AppendFormatted(str.AsSpan(), alignment, format);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(scoped text txt)
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(scoped text txt, int alignment, string? format = null)
    {
        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingRequired = alignment - txt.Length;
        if (paddingRequired <= 0)
        {
            // The value is as large or larger than the required amount of padding,
            // so just write the value.
            AppendFormatted(txt);
            return;
        }

        // Write the value along with the appropriate padding.
        EnsureCapacityForAdditionalChars(txt.Length + paddingRequired);
        if (leftAlign)
        {
            txt.CopyTo(_chars.Slice(_position));
            _position += txt.Length;
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
        }
        else
        {
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
            txt.CopyTo(_chars.Slice(_position));
            _position += txt.Length;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value)
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

        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value, string? format)
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
                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, format, default))
                {
                    GrowBy(1);
                }

                _position += charsWritten;
                return;
            }

            str = ((IFormattable)value).ToString(format, default); // constrained call avoiding boxing for value types
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value, int alignment)
    {
        int startingPos = _position;
        AppendFormatted(value);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<T>(T value, int alignment, string? format)
    {
        int startingPos = _position;
        AppendFormatted(value, format);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }

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
                _chars.Slice(_position, paddingNeeded).Fill(' ');
            }
            else
            {
                _chars.Slice(startingPos, charsWritten).CopyTo(_chars.Slice(startingPos + paddingNeeded));
                _chars.Slice(startingPos, paddingNeeded).Fill(' ');
            }

            _position += paddingNeeded;
        }
    }

    /// <summary>Ensures <see cref="_chars"/> has the capacity to store <paramref name="additionalChars"/> beyond <see cref="_position"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityForAdditionalChars(int additionalChars)
    {
        if (_chars.Length - _position < additionalChars)
        {
            GrowBy(additionalChars);
        }
    }

#endregion

#region IBufferWriter<char>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Advance(int count)
    {
        _position += count;
        if (_position > _chars.Length)
            throw new InvalidOperationException();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Memory<char> GetMemory(int sizeHint = 0)
    {
        int minCapacity = int.Min(sizeHint, 16);
        if (_rentedArray is null || AvailableLength < minCapacity)
        {
            GrowBy(minCapacity);
        }

        Debug.Assert(_rentedArray is not null);
        Debug.Assert(AvailableLength >= minCapacity);

        return _rentedArray.AsMemory(_position);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Span<char> GetSpan(int sizeHint = 0)
    {
        int minCapacity = int.Min(sizeHint, 16);
        if (AvailableLength < minCapacity)
        {
            GrowBy(minCapacity);
        }
        Debug.Assert(AvailableLength >= minCapacity);
        return _chars.Slice(_position);
    }

#endregion

#region Write

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
//
//    [OverloadResolutionPriority(100)]
//    public void Write<T>(T? value)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//        if (value is IFormattable)
//        {
//            // If the value can format itself directly into our buffer, do so.
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                // constrained call avoiding boxing for value types
//                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, default, default))
//                {
//                    GrowBy(1);
//                }
//
//                _position += charsWritten;
//                return;
//            }
//
//            // constrained call avoiding boxing for value types
//            str = ((IFormattable)value).ToString(default, default);
//        }
//        else
//        {
//            str = value.ToString();
//        }
//
//        Write(str);
//    }
//
//#if NET9_0_OR_GREATER
//    [OverloadResolutionPriority(75)]
//    public void Write<T>(T? value, Constrain.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        string? str = Anything.ToString<T>(value, _);
//        Write(str);
//    }
//#endif

#endregion

//#region Format
//
//    [OverloadResolutionPriority(100)]
//    public void Format<SF>(SF? value, text format, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasISpanFormattable<SF> _ = default)
//        where SF : ISpanFormattable
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        int charsWritten;
//
//        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format, provider))
//        {
//            GrowBy(1);
//        }
//
//        _position += charsWritten;
//    }
//
//    [OverloadResolutionPriority(90)]
//    public void Format<F>(F? value, string? format, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasIFormattable<F> _ = default)
//        where F : IFormattable
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str = value.ToString(format, provider);
//        Write(str);
//    }
//
//    [OverloadResolutionPriority(50)]
//    public void Format<T>(T? value, string? format, IFormatProvider? provider = default)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//        if (value is IFormattable)
//        {
//            // If the value can format itself directly into our buffer, do so.
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                // constrained call avoiding boxing for value types
//                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, format, provider))
//                {
//                    GrowBy(1);
//                }
//
//                _position += charsWritten;
//                return;
//            }
//
//            // constrained call avoiding boxing for value types
//            str = ((IFormattable)value).ToString(format, provider);
//        }
//        else
//        {
//            str = value.ToString();
//        }
//
//        Write(str);
//    }
//
//#if NET9_0_OR_GREATER
//    [OverloadResolutionPriority(30)]
//    public void Format<T>(T? value, string? format, IFormatProvider? provider = default, Constrain.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        string? str = Anything.Format<T>(value, format, provider);
//        Write(str);
//    }
//#endif
//
//#endregion
//
//#region Align
//
//#endregion


    [HandlesResourceDisposal]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = _rentedArray;

        // Defensive clear
        this = default;

        TextPool.Return(toReturn);
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = new string(Written);
        Dispose();
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => new string(Written);
}