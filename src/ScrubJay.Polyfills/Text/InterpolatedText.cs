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
[MustDisposeResource(true)]
[InterpolatedStringHandler]
public ref struct InterpolatedText : IDisposable
{
    private const int GUESSED_HOLE_LENGTH = 16;
    private const int MIN_ARRAY_LENGTH = 512;
    private const int STRING_MAX_LENGTH = 0x3FFFFFDF;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetMinCapacity(int literalLength, int formattedCount) =>
        Math.Max(MIN_ARRAY_LENGTH, literalLength + (formattedCount * GUESSED_HOLE_LENGTH));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetMinCapacity(int minCapacity) =>
        Math.Max(MIN_ARRAY_LENGTH, minCapacity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Return(char[]? array, bool clearArray = true)
    {
        if (array is not null && array.Length > 0)
        {
            ArrayPool<char>.Shared.Return(array, clearArray);
        }
    }

    private char[]? _arrayToReturnToPool;
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

    public InterpolatedText()
    {
        _chars = _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(MIN_ARRAY_LENGTH);
        _position = 0;
    }
    
    public InterpolatedText(int minCapacity)
    {
        _chars = _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(GetMinCapacity(minCapacity));
        _position = 0;
    }

    public InterpolatedText(Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _arrayToReturnToPool = null;
        _position = 0;
    }

    public InterpolatedText(int literalLength, int formattedCount)
    {
        _chars = _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(GetMinCapacity(literalLength, formattedCount));
        _position = 0;
    }

    public InterpolatedText(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _chars = initialBuffer;
        _arrayToReturnToPool = null;
        _position = 0;
    }

#region Grow
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowBy(int additionalChars)
    {
        Debug.Assert(additionalChars > _chars.Length - _position);
        GrowCore((uint)_position + (uint)additionalChars);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow()
    {
        GrowCore((uint)_chars.Length + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowCore(uint requiredMinCapacity)
    {
        uint newCapacity = Math.Max(requiredMinCapacity, Math.Min((uint)_chars.Length * 2, STRING_MAX_LENGTH));
        int arraySize = (int)Math.Clamp(newCapacity, MIN_ARRAY_LENGTH, int.MaxValue);

        char[] newArray = ArrayPool<char>.Shared.Rent(arraySize);
        _chars.Slice(0, _position).CopyTo(newArray);

        char[]? toReturn = _arrayToReturnToPool;
        _chars = _arrayToReturnToPool = newArray;
        Return(toReturn);
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
        EnsureCapacityForAdditionalChars(1 + paddingRequired);
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
        EnsureCapacityForAdditionalChars(str.Length + paddingRequired);
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
        EnsureCapacityForAdditionalChars(text.Length + paddingRequired);
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

#region AppendFormatted<TEnum>(TEnum @enum, ...?)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void AppendFormatted<TEnum>(TEnum @enum, TypeConstraints.IsEnum<TEnum> _ = default)
        where TEnum : struct, Enum
    {
        int charsWritten;
        while (!Enum.TryFormat<TEnum>(@enum, _chars.Slice(_position), out charsWritten, default))
        {
            Grow();
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
            Grow();
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
                    Grow();
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
                    Grow();
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
#endregion

    [HandlesResourceDisposal]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = _arrayToReturnToPool;
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