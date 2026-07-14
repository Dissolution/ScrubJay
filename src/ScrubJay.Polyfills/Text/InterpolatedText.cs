using System.Buffers;
using System.ComponentModel;
// ReSharper disable MergeCastWithTypeCheck
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Polyfills.Text;

[PublicAPI]
public delegate void InterpolatedTextWrite(ref InterpolatedText text);

[PublicAPI]
public delegate void InterpolatedTextWrite<in T>(ref InterpolatedText text, T value);

[PublicAPI]
public delegate void InterpolatedTextWrite<in T1, in T2>(ref InterpolatedText text, T1 arg1, T2 arg2);

[PublicAPI]
[InterpolatedStringHandler]
[MustDisposeResource(true)]
[StructLayout(LayoutKind.Auto)]
public ref struct InterpolatedText : IDisposable
{
    private const int MIN_CAPACITY = 512;
    private const int MAX_CAPACITY = 0x3FFFFFDF; // string.MaxLength

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char[] Rent(int minCapacity)
    {
        minCapacity = Math.Clamp(minCapacity, MIN_CAPACITY, MAX_CAPACITY);
        return ArrayPool<char>.Shared.Rent(minCapacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Return(char[]? array, bool clearArray = true)
    {
        if (array is not null && array.Length > 0)
        {
            ArrayPool<char>.Shared.Return(array, clearArray);
        }
    }

    private char[]? _charArray;
    private Span<char> _chars;
    private int _position;

    public readonly text Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(0, _position);
    }

    public readonly int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _position;
    }

    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Length;
    }

    [MustDisposeResource(false)]
    public InterpolatedText()
    {
        _chars = _charArray = [];
        _position = 0;
    }

    [MustDisposeResource(true)]
    public InterpolatedText(int minCapacity)
    {
        _chars = _charArray = Rent(minCapacity);
        _position = 0;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(Span<char> initialBuffer)
    {
        _charArray = null;
        _chars = initialBuffer;
        _position = 0;
    }

    [MustDisposeResource(true)]
    public InterpolatedText(int literalLength, int formattedCount)
    {
        _chars = _charArray = Rent(literalLength + (formattedCount * 16));
        _position = 0;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _charArray = null;
        _position = 0;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(int literalLength, int formattedCount, InterpolatedText parentText)
    {
        _charArray = parentText._charArray;
        _chars = parentText._chars;
        _position = parentText._position;
    }

#region Grow
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowCore(int minCapacity)
    {
        minCapacity = Math.Max(minCapacity, _chars.Length * 2);
        char[] newArray = Rent(minCapacity);

        _chars.Slice(0, _position).CopyTo(newArray);

        char[]? toReturn = _charArray;
        _chars = _charArray = newArray;
        Return(toReturn);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowBy(int additionalChars)
    {
        Debug.Assert(additionalChars > 0);
        Debug.Assert(additionalChars > _chars.Length - _position);
        GrowCore(_position + additionalChars);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowABit()
    {
        GrowCore(_chars.Length + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCanAdd(int count)
    {
        if (_chars.Length - _position < count)
        {
            GrowBy(count);
        }
    }
#endregion

#region Alignment
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
            EnsureCanAdd(paddingNeeded);

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
#endregion

#region InterpolatedStringHandler Methods
#region AppendLiteral(string)
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAppendString(string value)
    {
        GrowBy(value.Length);
        value.CopyTo(_chars.Slice(_position));
        _position += value.Length;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string value)
    {
        if (value.TryCopyTo(_chars.Slice(_position)))
        {
            _position += value.Length;
        }
        else
        {
            GrowAppendString(value);
        }
    }
#endregion

#region AppendFormatted(char)
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAppendChar(char ch)
    {
        GrowBy(1);
        int pos = _position;
        _chars[pos] = ch;
        _position = pos + 1;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(char ch)
    {
        if (_position < _chars.Length)
        {
            _chars[_position] = ch;
            _position++;
        }
        else
        {
            GrowAppendChar(ch);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(char ch, int alignment, string? _ = default)
    {
        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingRequired = alignment - 1;
        if (paddingRequired <= 0)
        {
            // The value is as large or larger than the required amount of padding,
            // so just write the value.
            AppendFormatted(ch);
            return;
        }

        // Write the value along with the appropriate padding.
        EnsureCanAdd(1 + paddingRequired);
        if (leftAlign)
        {
            _chars[_position] = ch;
            _position++;
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
        }
        else
        {
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
            _chars[_position] = ch;
            _position++;
        }
    }
#endregion

#region AppendFormatted(string? str)
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(string? str)
    {
        if (str is null)
            return;

        if (str.TryCopyTo(_chars.Slice(_position)))
        {
            _position += str.Length;
        }
        else
        {
            GrowAppendString(str);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(string? str, int alignment, string? _ = null)
    {
        if (str is null)
            return;

        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingRequired = alignment - str.Length;
        if (paddingRequired <= 0)
        {
            // The value is as large or larger than the required amount of padding,
            // so just write the value.
            AppendLiteral(str);
            return;
        }

        // Write the value along with the appropriate padding.
        EnsureCanAdd(str.Length + paddingRequired);
        if (leftAlign)
        {
            str.CopyTo(_chars.Slice(_position));
            _position += str.Length;
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
        }
        else
        {
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
            str.CopyTo(_chars.Slice(_position));
            _position += str.Length;
        }
    }
#endregion

#region AppendFormatted(scoped text text)
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAppendSpan(scoped text text)
    {
        GrowBy(text.Length);
        text.CopyTo(_chars.Slice(_position));
        _position += text.Length;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(scoped text text)
    {
        // Fast path for when the value fits in the current buffer
        if (text.TryCopyTo(_chars.Slice(_position)))
        {
            _position += text.Length;
        }
        else
        {
            GrowAppendSpan(text);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted(scoped text text, int alignment, string? _ = null)
    {
        bool leftAlign = false;
        if (alignment < 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        int paddingRequired = alignment - text.Length;
        if (paddingRequired <= 0)
        {
            // The value is as large or larger than the required amount of padding,
            // so just write the value.
            AppendFormatted(text);
            return;
        }

        // Write the value along with the appropriate padding.
        EnsureCanAdd(text.Length + paddingRequired);
        if (leftAlign)
        {
            text.CopyTo(_chars.Slice(_position));
            _position += text.Length;
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
        }
        else
        {
            _chars.Slice(_position, paddingRequired).Fill(' ');
            _position += paddingRequired;
            text.CopyTo(_chars.Slice(_position));
            _position += text.Length;
        }
    }
#endregion

#region AppendFormatted - nested Interpolation
    public void AppendFormatted(
        [HandlesResourceDisposal] [InterpolatedStringHandlerArgument("")]
        ref InterpolatedText interpolated)
    {
        // take back what interpolated took and used
        _charArray = interpolated._charArray;
        _chars = interpolated._chars;
        _position = interpolated._position;
    }
#endregion

#region AppendFormatted<TEnum>(TEnum @enum, ...?)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<TEnum>(TEnum @enum, TypeConstraints.IsEnum<TEnum> _ = default)
        where TEnum : struct, Enum
    {
        int charsWritten;
        while (!Enum.TryFormat<TEnum>(@enum, _chars.Slice(_position), out charsWritten, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<TEnum>(TEnum @enum,
        [StringSyntax("EnumFormat")] string? format, TypeConstraints.IsEnum<TEnum>? _ = default)
        where TEnum : struct, Enum
    {
        int charsWritten;
        while (!Enum.TryFormat<TEnum>(@enum, _chars.Slice(_position), out charsWritten, format))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endregion

#region AppendFormatted primitives
    public void AppendFormatted(bool boolean)
    {
        if (boolean)
        {
            AppendLiteral("true");
        }
        else
        {
            AppendLiteral("false");
        }
    }

    public void AppendFormatted(byte u8)
    {
        int charsWritten;
        while (!u8.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(byte u8,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!u8.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(sbyte i8)
    {
        int charsWritten;
        while (!i8.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(sbyte i8,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!i8.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(short i16)
    {
        int charsWritten;
        while (!i16.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(short i16,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!i16.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(ushort u16)
    {
        int charsWritten;
        while (!u16.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(ushort u16,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!u16.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(int i32)
    {
        int charsWritten;
        while (!i32.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(int i32,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!i32.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(uint u32)
    {
        int charsWritten;
        while (!u32.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(uint u32,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!u32.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(long i64)
    {
        int charsWritten;
        while (!i64.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(long i64,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!i64.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(ulong u64)
    {
        int charsWritten;
        while (!u64.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(ulong u64,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!u64.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

#if NET7_0_OR_GREATER
    public void AppendFormatted(Int128 i128)
    {
        int charsWritten;
        while (!i128.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(Int128 i128,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!i128.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(UInt128 u128)
    {
        int charsWritten;
        while (!u128.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(UInt128 u128,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!u128.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endif

#if NET6_0_OR_GREATER
    public void AppendFormatted(Half f16)
    {
        int charsWritten;
        while (!f16.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(Half f16,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!f16.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endif

    public void AppendFormatted(float f32)
    {
        int charsWritten;
        while (!f32.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(float f32,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!f32.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(double f64)
    {
        int charsWritten;
        while (!f64.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(double f64,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!f64.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(decimal dec)
    {
        int charsWritten;
        while (!dec.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(decimal dec,
        [StringSyntax("NumericFormat")] string? format)
    {
        int charsWritten;
        while (!dec.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(Guid value)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(Guid value,
        [StringSyntax("GuidFormat")] string? format)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

#if NET6_0_OR_GREATER
    public void AppendFormatted(DateOnly date)
    {
        int charsWritten;
        while (!date.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(DateOnly date,
        [StringSyntax("DateOnlyFormat")] string? format)
    {
        int charsWritten;
        while (!date.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endif

    public void AppendFormatted(DateTime value)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(DateTime value,
        [StringSyntax("DateTimeFormat")] string? format)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(DateTimeOffset value)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(DateTimeOffset value,
        [StringSyntax("DateTimeFormat")] string? format)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

#if NET6_0_OR_GREATER
    public void AppendFormatted(TimeOnly time)
    {
        int charsWritten;
        while (!time.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(TimeOnly time,
        [StringSyntax("TimeOnlyFormat")] string? format)
    {
        int charsWritten;
        while (!time.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endif

    public void AppendFormatted(TimeSpan value)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, default, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }

    public void AppendFormatted(TimeSpan value,
        [StringSyntax("TimeSpanFormat")] string? format)
    {
        int charsWritten;
        while (!value.TryFormat(_chars.Slice(_position), out charsWritten, format, default))
        {
            GrowABit();
        }
        _position += charsWritten;
    }
#endregion

#region AppendFormatted<T>(T? value, ...?)
    [EditorBrowsable(EditorBrowsableState.Never)]
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
                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, default, default))
                {
                    GrowABit();
                }

                _position += charsWritten;
                return;
            }
#endif

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
                while (!((ISpanFormattable)value).TryFormat(_chars.Slice(_position), out charsWritten, format, default))
                {
                    GrowABit();
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T? value, int alignment)
    {
        int startingPos = _position;
        AppendFormatted(value);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T? value, int alignment, string? format)
    {
        int startingPos = _position;
        AppendFormatted(value, format);
        if (alignment != 0)
        {
            AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
        }
    }
#endregion
#endregion

#region Write methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(char ch) => AppendFormatted(ch);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(string? str) => AppendFormatted(str);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(scoped text text) => AppendFormatted(text);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write([HandlesResourceDisposal] [InterpolatedStringHandlerArgument("")] ref InterpolatedText interpolatedText)
        => AppendFormatted(ref interpolatedText);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write<T>(T? value) => AppendFormatted<T>(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Format<T>(T? value, string? format) => AppendFormatted<T>(value, format);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Format<T>(T? value, string? format, int alignment) => AppendFormatted<T>(value, alignment, format);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Align(char ch, int alignment) => AppendFormatted(ch, alignment);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Align(string? str, int alignment) => AppendFormatted(str, alignment);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Align(scoped text text, int alignment) => AppendFormatted(text, alignment);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Align<T>(T? value, int alignment) => AppendFormatted<T>(value, alignment);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Align<T>(T? value, int alignment, string? format) => AppendFormatted<T>(value, alignment, format);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void NewLine() => AppendFormatted(Environment.NewLine);

    public void Fill(int count, char ch)
    {
        if (count <= 0) return;
        EnsureCanAdd(count);
        _chars.Slice(_position, count).Fill(ch);
        _position += count;
    }
#endregion

    [HandlesResourceDisposal]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = _charArray;
        this = default;
        Return(toReturn);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString()
    {
#if NETFRAMEWORK || NETSTANDARD2_0
        return _chars.Slice(0, _position).ToString();
#else
        return new string(_chars.Slice(0, _position));
#endif
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }
}