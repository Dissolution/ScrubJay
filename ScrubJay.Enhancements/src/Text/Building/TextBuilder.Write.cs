using ScrubJay.Enhancements.Text.Utilities;

namespace ScrubJay.Enhancements.Text.Building;

/* This portion of TextBuilder contains the underlying methods that write text directly to the rented array
 * These are designed for efficiency and do not return TextBuilder fluently for better inlining
 */

public ref partial struct TextBuilder
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAndWrite(scoped text text)
    {
        int len = text.Length;
        GrowBy(len);
        TextHelper.Notsafe.CopyText(text, ref _charSpan.UnsafeRef(_position), len);
        _position += len;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(in char ch)
    {
        if (_position < _charSpan.Length)
        {
            _charSpan[_position] = ch;
            _position++;
        }
        else
        {
            GrowAndWrite(ch.AsSpan());
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(scoped text text)
    {
        if (text.TryCopyTo(Available))
        {
            _position += text.Length;
        }
        else
        {
            GrowAndWrite(text);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(string? str) => Write(str.AsSpan());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(
#if !NETFRAMEWORK && !NETSTANDARD
        [InterpolatedStringHandlerArgument("")]
#endif
        ref InterpolatedTextBuilder interpolatedText)
    {
        // already written
        return;
    }

    public void Write<T>(T? value)
    {
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, default))
                {
                    Grow();
                }

                _position += charsWritten;
                return;
            }
#else
            Write(((IFormattable)value).ToString(null, null));
#endif
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }
    }

#if NET9_0_OR_GREATER
    public void Write<T>(in T value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        Write(Any.ToString(in value));
    }
#endif
}