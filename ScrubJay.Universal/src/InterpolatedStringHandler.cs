//// ReSharper disable MergeCastWithTypeCheck
//// ReSharper disable UnusedParameter.Local
//
//using System.Buffers;
//using System.ComponentModel;
//using ScrubJay.Universal.Extensions;
//
//namespace ScrubJay.Universal;
//
//[InterpolatedStringHandler]
//[StructLayout(LayoutKind.Auto)]
//[MustDisposeResource]
//public ref struct InterpolatedStringHandler : IDisposable
//{
//    private char[]? _rentedCharArray;
//    private Span<char> _chars;
//    private int _position;
//
//    internal Span<char> Available
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _chars.Slice(_position);
//    }
//
//    public text Written
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _chars.Slice(0, _position);
//    }
//
//    public InterpolatedStringHandler(int literalLength, int formattedCount)
//    {
//        _chars = _rentedCharArray = ArrayPool<char>.Shared.Rent(literalLength + (formattedCount * 16));
//        _position = 0;
//    }
//
//    public InterpolatedStringHandler(int literalLength, int formattedCount, Span<char> initialBuffer)
//    {
//        _chars = initialBuffer;
//        _rentedCharArray = null;
//        _position = 0;
//    }
//
//#region Grow
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private void GrowImpl(int requiredMinCapacity)
//    {
//        int newCapacity = Math.Max(requiredMinCapacity, Math.Min(_chars.Length * 2, 0x3FFFFFDF));
//        int arraySize = Math.Clamp(newCapacity, 256, int.MaxValue);
//
//        char[] newArray = ArrayPool<char>.Shared.Rent(arraySize);
//        _chars.Slice(0, _position).CopyTo(newArray);
//
//        char[]? toReturn = _rentedCharArray;
//        _chars = _rentedCharArray = newArray;
//
//        if (toReturn is not null && toReturn.Length > 0)
//        {
//            ArrayPool<char>.Shared.Return(toReturn);
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowByOne()
//    {
//        GrowImpl(_chars.Length + 1);
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowBy(int additionalChars)
//    {
//        Debug.Assert(additionalChars > _chars.Length - _position);
//        GrowImpl(_position + additionalChars);
//    }
//#endregion Grow
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private void EnsureCapacityForAdditionalChars(int additionalChars)
//    {
//        if (_chars.Length - _position < additionalChars)
//        {
//            GrowBy(additionalChars);
//        }
//    }
//
//
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
//                _chars.Slice(_position, paddingNeeded).Fill(' ');
//            }
//            else
//            {
//                _chars.Slice(startingPos, charsWritten).CopyTo(_chars.Slice(startingPos + paddingNeeded));
//                _chars.Slice(startingPos, paddingNeeded).Fill(' ');
//            }
//
//            _position += paddingNeeded;
//        }
//    }
//
//
//
//#region InterpolatedStringHandler Methods
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowAppendString(string str)
//    {
//        Debug.Assert(str is not null);
//        GrowBy(str!.Length);
//        str
//#if NETFRAMEWORK || NETSTANDARD
//            .AsSpan()
//#endif
//            .CopyTo(_chars.Slice(_position));
//        _position += str.Length;
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendLiteral(string str)
//    {
//        if (str.TryCopyTo(_chars.Slice(_position)))
//        {
//            _position += str.Length;
//        }
//        else
//        {
//            GrowAppendString(str);
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowAppendNullableString(string? str)
//    {
//        if (str is not null)
//        {
//            GrowBy(str.Length);
//            str
//#if NETFRAMEWORK || NETSTANDARD
//                .AsSpan()
//#endif
//                .CopyTo(_chars.Slice(_position));
//            _position += str.Length;
//        }
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted(string? str)
//    {
//        if (str is not null && str.TryCopyTo(_chars.Slice(_position)))
//        {
//            _position += str.Length;
//        }
//        else
//        {
//            GrowAppendNullableString(str);
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowAppendText(scoped text text)
//    {
//        GrowBy(text.Length);
//        text.CopyTo(_chars.Slice(_position));
//        _position += text.Length;
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(scoped text text)
//    {
//        if (text.TryCopyTo(_chars.Slice(_position)))
//        {
//            _position += text.Length;
//        }
//        else
//        {
//            GrowAppendText(text);
//        }
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted<T>(T? value)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//        if (value is IFormattable)
//        {
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, default, default))
//                {
//                    GrowByOne();
//                }
//
//                _position += charsWritten;
//                return;
//            }
//
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
//#if NET9_0_OR_GREATER
//    public void AppendFormatted<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        if (Any.HasTryFormat<T>())
//        {
//            int charsWritten;
//            while (!Any.TryFormat(in value, _chars.Slice(_position), out charsWritten))
//            {
//                GrowByOne();
//            }
//
//            _position += charsWritten;
//            return;
//        }
//
//        // Any.Format falls back to Any.ToString
//        string str = Any.Format<T>(in value);
//        AppendLiteral(str);
//    }
//#endif
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted<T>(T? value, string? format)
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        string? str;
//        if (value is IFormattable)
//        {
//            if (value is ISpanFormattable)
//            {
//                int charsWritten;
//                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, format, default))
//                {
//                    GrowByOne();
//                }
//
//                _position += charsWritten;
//                return;
//            }
//
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
//#if NET9_0_OR_GREATER
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted<T>(in T? value, string? format, TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        if (value is null)
//        {
//            return;
//        }
//
//        if (Any.HasTryFormat<T>())
//        {
//            int charsWritten;
//            while (!Any.TryFormat(in value, _chars.Slice(_position), out charsWritten, format))
//            {
//                GrowByOne();
//            }
//
//            _position += charsWritten;
//            return;
//        }
//
//        // Any.Format falls back to Any.ToString
//        string str = Any.Format<T>(in value, format);
//        AppendLiteral(str);
//    }
//#endif
//
//#region w/Alignment
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted(string? value, int alignment) => AppendFormatted(value.AsSpan(), alignment);
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted(scoped text value, int alignment)
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
//            value.CopyTo(_chars.Slice(_position));
//            _position += value.Length;
//            _chars.Slice(_position, paddingRequired).Fill(' ');
//            _position += paddingRequired;
//        }
//        else
//        {
//            _chars.Slice(_position, paddingRequired).Fill(' ');
//            _position += paddingRequired;
//            value.CopyTo(_chars.Slice(_position));
//            _position += value.Length;
//        }
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted<T>(T? value, int alignment)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        int startingPos = _position;
//        AppendFormatted(value);
//        if (alignment != 0)
//        {
//            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
//        }
//    }
//
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void AppendFormatted<T>(T? value, int alignment, string? format)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        int startingPos = _position;
//        AppendFormatted(value, format);
//        if (alignment != 0)
//        {
//            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
//        }
//    }
//#endregion w/Alignment
//#endregion
//
//#region Write Methods
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Write(in char ch)
//    {
//        if (_position < _chars.Length)
//        {
//            _chars[_position++] = ch;
//        }
//        else
//        {
//            GrowAppendText(CharacterExtensions.AsSpan(in ch));
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Write(scoped text text) => AppendFormatted(text);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Write(string? str) => AppendFormatted(str);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Write<T>(T? value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => AppendFormatted<T>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Format<T>(T? value, string? format)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => AppendFormatted<T>(value, format);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Align(in char ch, int alignment) => AppendFormatted(CharacterExtensions.AsSpan(in ch), alignment);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Align(scoped text text, int alignment) => AppendFormatted(text, alignment);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Align(string? str, int alignment) => AppendFormatted(str, alignment);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Align<T>(T? value, int alignment)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => AppendFormatted(value, alignment);
//#endregion
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    [HandlesResourceDisposal]
//    public void Dispose()
//    {
//        char[]? toReturn = _rentedCharArray;
//
//        this = default;
//        if (toReturn is not null && toReturn.Length > 0)
//        {
//            ArrayPool<char>.Shared.Return(toReturn);
//        }
//    }
//
//    public override string ToString() => Written.ToString();
//
//    [HandlesResourceDisposal]
//    public string ToStringAndDispose()
//    {
//        string result = Written.ToString();
//        Dispose();
//        return result;
//    }
//}