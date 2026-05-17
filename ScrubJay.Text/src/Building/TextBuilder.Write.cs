namespace ScrubJay.Text.Building;

/* This portion of TextBuilder contains the underlying methods that write text directly to the rented array
 * These are designed for efficiency and do not return TextBuilder fluently for better inlining
 */

public partial class TextBuilder
{

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenWrite(char ch)
    {
        GrowBy(1);
        _chars[_position++] = ch;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Write(char ch)
    {
        if (_position < _chars.Length)
        {
            _chars[_position++] = ch;
        }
        else
        {
            GrowThenWrite(ch);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenWrite(scoped text text)
    {
        GrowBy(text.Length);
        TextHelper.Unsafe.CopyTo(text, _chars.AsSpan(_position), text.Length);
        _position += text.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Write(scoped text text)
    {
        if (_position + text.Length < _chars.Length)
        {
            TextHelper.Unsafe.CopyTo(text, _chars.AsSpan(_position), text.Length);
            _position += text.Length;
        }
        else
        {
            GrowThenWrite(text);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Write(string? str)
    {
        if (str is not null)
        {
            if (_position + str.Length < _chars.Length)
            {
                TextHelper.Unsafe.CopyTo(str, _chars.AsSpan(_position), str.Length);
                _position += str.Length;
            }
            else
            {
                GrowThenWrite(str);
            }
        }
    }

    internal void Write<T>(T? value)
    {
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, null))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(null, null));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    internal void Write<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => Write(Any.ToString<T>(in value));
#endif
}