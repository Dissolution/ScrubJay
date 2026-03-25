//#pragma warning disable S3247, RCS1220
//// ReSharper disable MergeCastWithTypeCheck
//// ReSharper disable MethodOverloadWithOptionalParameter
//
//
//using ScrubJay.Text.Pooling;
//using ScrubJay.Text.Utilities;
//
//namespace ScrubJay.Text;
//
//[PublicAPI]
//[InterpolatedStringHandler]
//[MustDisposeResource(false)]
//public ref struct InterpolatedTextBuilder
//{
//    private TextBuilder _textBuilder;
//
//    public Span<char> Written => _textBuilder.Written;
//
//    internal Span<char> Available => _textBuilder.Available;
//
//    public readonly int Length => _textBuilder.Length;
//
//    public readonly int Capacity => _textBuilder.Capacity;
//
//    public InterpolatedTextBuilder(int literalLength, int formattedCount, ref TextBuilder builder)
//    {
//        _textBuilder = builder;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendLiteral(string str) => _textBuilder.Write(str);
//
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(ref readonly char ch) => _textBuilder.Write(in ch);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(scoped text text) => _textBuilder.Write(text);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(string? str) => _textBuilder.Write(str);
//
//    public void AppendFormatted<T>(T? value) => _textBuilder.Write<T>(value);
//
//#if NET9_0_OR_GREATER
//    public void AppendFormatted<T>(ref readonly T value, TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//        => _textBuilder.Write<T>(in value, _);
//#endif
//
//    public void AppendFormatted<T>(
//        T? value,
//        string? format)
//        => _textBuilder.Format<T>(value, format);
//
//    public void AppendFormatted<T>(
//        T? value,
//        scoped ReadOnlySpan<char> format,
//        IFormatProvider? formatProvider = null)
//        => _textBuilder.Format<T>(value, format);
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