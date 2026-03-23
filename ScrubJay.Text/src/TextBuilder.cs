//using static InlineIL.IL;
//using ScrubJay.Text.Pooling;
//using ScrubJay.Text.Utilities;
//// ReSharper disable MergeCastWithTypeCheck
//
//namespace ScrubJay.Text;
//
//public static class TextBuilderExtensions
//{
//    public static ref TextBuilder Ref(this ref TextBuilder builder)
//    {
//        return ref builder;
//    }
//}
//
//[PublicAPI]
//[MustDisposeResource(true)]
//public ref struct TextBuilder
//{
//    private char[]? _charArray;
//
//    private int _capacity;
//    private unsafe char* _firstChar;
//    private int _position;
//
//
//    public Span<char> Written
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get
//        {
//            unsafe
//            {
//                return new Span<char>(_firstChar, _position);
//            }
//        }
//    }
//
//    public Span<char> Available
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get
//        {
//            unsafe
//            {
//                return new Span<char>(Unsafe.Add<char>(_firstChar, _position), _capacity - _position);
//            }
//        }
//    }
//
//    public readonly int Length
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _position;
//    }
//
//    public readonly int Capacity
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _capacity;
//    }
//
//    public TextBuilder()
//    {
//        _charArray = TextPool.Rent(1024);
//        _capacity = _charArray.Length;
//        _position = 0;
//        unsafe
//        {
//            _firstChar = (char*)Unsafe.AsPointer<char>(ref MemoryMarshal.GetArrayDataReference(_charArray));
//        }
//    }
//
//#region Grow
//
//    private void Grow(int newCapacity)
//    {
//        Debug.Assert(newCapacity > Capacity);
//        var newArray = TextPool.Rent(newCapacity);
//        TextHelper.Notsafe.CopyBlock(in _firstChar, newArray, _position);
//    }
//
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowThenCopyString(string str)
//    {
//        int len = str.Length;
//        GrowBy(len);
//        TextHelper.Notsafe.CopyBlock(str, Available, len);
//        _position += len;
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowThenCopySpan(scoped text text)
//    {
//        int len = text.Length;
//        GrowBy(len);
//        TextHelper.Notsafe.CopyBlock(text, Available, len);
//        _position += len;
//    }
//#endregion
//
//    public ref TextBuilder Append(ref readonly char ch)
//    {
//        if (_position < _charSpan.Length)
//        {
//            _charSpan[_position] = ch;
//            _position++;
//        }
//        else
//        {
//            GrowThenCopySpan(ch.AsSpan());
//        }
//        Emit.Ldarg_0();
//        return ref ReturnRef<TextBuilder>();
//    }
//
//    public ref TextBuilder Append(scoped text text)
//    {
//        if (TextHelper.TryCopyTo(text, Available))
//        {
//            _position += text.Length;
//        }
//        else
//        {
//            GrowThenCopySpan(text);
//        }
//        Emit.Ldarg_0();
//        return ref ReturnRef<TextBuilder>();
//    }
//
//    public ref TextBuilder Append(string? str)
//    {
//        if (str is not null)
//        {
//            int len = str.Length;
//            int newLen = _position + len;
//            if (newLen <= _charSpan.Length)
//            {
//                TextHelper.Notsafe.CopyBlock(str, ref Unsafe.Add<char>(ref _charSpan.GetPinnableReference(), _position), len);
//                _position = newLen;
//            }
//            else
//            {
//                GrowThenCopySpan(str.AsSpan());
//            }
//        }
//        Emit.Ldarg_0();
//        return ref ReturnRef<TextBuilder>();
//    }
//
//    public void Append<T>(T value)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//
//        if (value is IFormattable)
//        {
//#if NET6_0_OR_GREATER
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                // constrained call avoiding boxing for value types
//                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, default))
//                {
//                    Grow();
//                }
//
//                _position += charsWritten;
//                return;
//            }
//#endif
//
//            // constrained call avoiding boxing for value types
//            str = ((IFormattable)value).ToString(default, default);
//        }
//        else
//        {
//            str = value.ToString();
//        }
//
//        if (str is not null)
//        {
//            AppendLiteral(str);
//        }
//    }
//
//    public void Append<T>(T value, string? format)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//
//        if (value is IFormattable)
//        {
//#if NET6_0_OR_GREATER
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                // constrained call avoiding boxing for value types
//                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, default))
//                {
//                    Grow();
//                }
//
//                _position += charsWritten;
//                return;
//            }
//#endif
//            // constrained call avoiding boxing for value types
//            str = ((IFormattable)value).ToString(format, default);
//        }
//        else
//        {
//            str = value.ToString();
//        }
//
//        if (str is not null)
//        {
//            AppendLiteral(str);
//        }
//    }
//
//    /*
//
//
//
//
//
//    /// <summary>Writes the specified value to the handler.</summary>
//    /// <param name="value">The value to write.</param>
//    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
//    /// <typeparam name="T">The type of the value to write.</typeparam>
//    public void AppendFormatted<T>(T value, int alignment)
//    {
//        int startingPos = _position;
//        AppendFormatted(value);
//        if (alignment != 0)
//        {
//            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
//        }
//    }
//
//    /// <summary>Writes the specified value to the handler.</summary>
//    /// <param name="value">The value to write.</param>
//    /// <param name="format">The format string.</param>
//    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
//    /// <typeparam name="T">The type of the value to write.</typeparam>
//    public void AppendFormatted<T>(T value, int alignment, string? format)
//    {
//        int startingPos = _position;
//        AppendFormatted(value, format);
//        if (alignment != 0)
//        {
//            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
//        }
//    }
//
//
//
//    /// <summary>Writes the specified string of chars to the handler.</summary>
//    /// <param name="value">The span to write.</param>
//    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
//    /// <param name="format">The format string.</param>
//    public void AppendFormatted(scoped text value, int alignment = 0, string? format = null)
//    {
//        bool leftAlign = false;
//        if (alignment < 0)
//        {
//            leftAlign = true;
//            alignment = -alignment;
//        }
//
//        int paddingRequired = alignment - value.Length;
//        if (paddingRequired <= 0)
//        {
//            // The value is as large or larger than the required amount of padding,
//            // so just write the value.
//            AppendFormatted(value);
//            return;
//        }
//
//        // Write the value along with the appropriate padding.
//        EnsureCapacityForAdditionalChars(value.Length + paddingRequired);
//        if (leftAlign)
//        {
//            value.CopyTo(_charSpan.Slice(_position));
//            _position += value.Length;
//            _charSpan.Slice(_position, paddingRequired).Fill(' ');
//            _position += paddingRequired;
//        }
//        else
//        {
//            _charSpan.Slice(_position, paddingRequired).Fill(' ');
//            _position += paddingRequired;
//            value.CopyTo(_charSpan.Slice(_position));
//            _position += value.Length;
//        }
//    }
//
//    /// <summary>Writes the specified value to the handler.</summary>
//    /// <param name="value">The value to write.</param>
//    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
//    /// <param name="format">The format string.</param>
//    public void AppendFormatted(string? value, int alignment = 0, string? format = null) =>
//        // Format is meaningless for strings and doesn't make sense for someone to specify.  We have the overload
//        // simply to disambiguate between ROS<char> and object, just in case someone does specify a format, as
//        // string is implicitly convertible to both. Just delegate to the T-based implementation.
//        AppendFormatted<string?>(value, alignment, format);
//
//
//    /// <summary>Handles adding any padding required for aligning a formatted value in an interpolation expression.</summary>
//    /// <param name="startingPos">The position at which the written value started.</param>
//    /// <param name="alignment">Non-zero minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
//    private void AppendOrInsertAlignmentIfNeeded(int startingPos, int alignment)
//    {
//        Debug.Assert(startingPos >= 0 && startingPos <= _position);
//        Debug.Assert(alignment != 0);
//
//        int charsWritten = _position - startingPos;
//
//        bool leftAlign = false;
//        if (alignment < 0)
//        {
//            leftAlign = true;
//            alignment = -alignment;
//        }
//
//        int paddingNeeded = alignment - charsWritten;
//        if (paddingNeeded > 0)
//        {
//            EnsureCapacityForAdditionalChars(paddingNeeded);
//
//            if (leftAlign)
//            {
//                _charSpan.Slice(_position, paddingNeeded).Fill(' ');
//            }
//            else
//            {
//                _charSpan.Slice(startingPos, charsWritten).CopyTo(_charSpan.Slice(startingPos + paddingNeeded));
//                _charSpan.Slice(startingPos, paddingNeeded).Fill(' ');
//            }
//
//            _position += paddingNeeded;
//        }
//    }
//
//    /// <summary>Ensures <see cref="_charSpan"/> has the capacity to store <paramref name="additionalChars"/> beyond <see cref="_position"/>.</summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private void EnsureCapacityForAdditionalChars(int additionalChars)
//    {
//        if (_charSpan.Length - _position < additionalChars)
//        {
//            GrowBy(additionalChars);
//        }
//    }
//
//    */
//
//
//
//
//
//    [HandlesResourceDisposal]
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Dispose()
//    {
//        char[]? toReturn = _charArray;
//
//        // Defensive clear
//        this = default;
//
//        TextPool.Return(toReturn);
//    }
//
//    [HandlesResourceDisposal]
//    public string ToStringAndDispose()
//    {
//        string result = ToString();
//        Dispose();
//        return result;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public readonly override string ToString() => _charSpan.Slice(0, _position).ToString();
//}