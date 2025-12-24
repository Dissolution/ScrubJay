// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using ScrubJay.Extensions;
using ScrubJay.Universal;

// ReSharper disable MergeCastWithTypeCheck

namespace ScrubJay.Text;

[PublicAPI]
[InterpolatedStringHandler]
[MustDisposeResource]
public ref struct InterpolatedTextHandler : IDisposable
{
    internal const char ELLIPSIS = '…';

    [MustDisposeResource]
    public static implicit operator InterpolatedTextHandler(string? str)
    {
        if (str is null)
        {
            return default;
        }
        
        var handler = new InterpolatedTextHandler(str.Length, 0);
        handler.AppendLiteral(str);
        return handler;
    }
    
    [MustDisposeResource]
    public static implicit operator InterpolatedTextHandler(text text)
    {
        var handler = new InterpolatedTextHandler(text.Length, 0);
        handler.AppendFormatted(text);
        return handler;
    }

    internal char[]? _rentedCharArray;
    internal Span<char> _charSpan;
    internal int _position;

    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(_position);
    }

    internal ReadOnlySpan<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Slice(0, _position);
    }

    internal int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charSpan.Length;
    }


    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _position;
    }

    [MustDisposeResource]
    public InterpolatedTextHandler()
    {
        _charSpan = _rentedCharArray = null;
        _position = 0;
    }

    [MustDisposeResource]
    public InterpolatedTextHandler(int literalLength, int formattedCount)
    {
        int minCapacity = literalLength + (formattedCount * 16);
        _charSpan = _rentedCharArray = ArrayPool<char>.Shared.Rent(minCapacity);
        _position = 0;
    }

    [MustDisposeResource]
    public InterpolatedTextHandler(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _charSpan = initialBuffer;
        _rentedCharArray = null;
        _position = 0;
    }

    #region Growing
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowCore(int requiredMinCapacity)
    {
        char[] newArray = ArrayPool<char>.Shared.Rent(requiredMinCapacity * 2);
        Written.CopyTo(newArray);

        char[]? toReturn = _rentedCharArray;
        _charSpan = _rentedCharArray = newArray;

        if (toReturn is not null && toReturn.Length > 0)
        {
            ArrayPool<char>.Shared.Return(toReturn, true);
        }
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowBy(int adding)
    {
        GrowCore(Capacity + adding);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowTo(int minCapacity)
    {
        GrowCore(minCapacity);
    }

    public void EnsureCapacityFor(int count)
    {
        if (count <= 0 || _position + count <= Capacity)
            return;
        GrowBy(count);
    }
    #endregion

    public void AppendLiteral(string value)
    {
        if (_position + value.Length > Capacity)
            GrowBy(value.Length);
        value.CopyTo(Available);
        _position += value.Length;
    }

    public void AppendFormatted(char ch)
    {
        if (_position >= Capacity)
            GrowBy(1);
        _charSpan[_position] = ch;
        _position++;
    }

    public void AppendFormatted(string? str)
    {
        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

    public void AppendFormatted(scoped ReadOnlySpan<char> value)
    {
        if (_position + value.Length > Capacity)
            GrowBy(value.Length);
        value.CopyTo(Available);
        _position += value.Length;
    }

    public void AppendFormatted(Type? type)
    {
        AppendLiteral(TypeName.For(type));
    }

    public void AppendFormatted<T>(T? value)
    {
        if (value is null)
        {
            return;
        }

        if (value is Type type)
        {
            AppendLiteral(TypeName.For(type));
            return;
        }

        string? str;

        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            // If the value can format itself directly into our buffer, do so.
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(_charSpan.Slice(_position), out charsWritten, default,
                           default))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
                return;
            }
#endif

            str = ((IFormattable)value).ToString();
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
    public void AppendFormatted<T>(T value, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        AppendLiteral(Any.ToString(value));
    }
#else
    public void AppendFormatted<T>(scoped ReadOnlySpan<T> span)
    {
        AppendLiteral(span.ToString());
    }

    public void AppendFormatted<T>(scoped Span<T> span)
    {
        AppendLiteral(span.ToString());
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
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, default))
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

#region with Alignment

// +width => right align
// -width => left align


    public void AppendFormatted(char ch, int alignment)
    {
        if (alignment == 0)
            return;

        // right align
        if (alignment > 0)
        {
            var padding = alignment - 1;
            EnsureCapacityFor(alignment);
            var avail = Available;
            avail[..padding].Fill(' ');
            avail[padding] = ch;
        }
        else
        {
            // left align
            alignment = -alignment;
            var padding = alignment - 1;
            EnsureCapacityFor(alignment);
            var avail = Available;
            avail[0] = ch;
            avail[1..(1 + padding)].Fill(' ');
        }

        Debug.Assert(alignment > 0);
        _position += alignment;
    }

    public void AppendFormatted(scoped ReadOnlySpan<char> text, int alignment)
    {
        int len = text.Length;
        if (alignment == 0)
            return;

        int padding;
        bool leftAlign = false;

        if (alignment <= 0)
        {
            leftAlign = true;
            alignment = -alignment;
        }

        padding = alignment - len;
        if (padding == 0)
        {
            AppendFormatted(text);
            return;
        }
        
        EnsureCapacityFor(alignment);
        var avail = _charSpan.Slice(_position, alignment);

        // right align
        if (!leftAlign)
        {
            if (padding > 0)
            {
                avail[..padding].Fill(' ');
                avail[padding..].FillFrom(text);
            }
            else
            {
                // truncate
                avail[0] = ELLIPSIS;
                avail[1..].FillFrom(text.Slice(0, alignment - 1));
            }
        }
        // left align
        else
        {
            if (padding > 0)
            {
                avail.FillFrom(text);
                avail[len..].Fill(' ');
            }
            else
            {
                avail.FillFrom(text.Slice(0, alignment - 1));
                avail[alignment-1] = ELLIPSIS;
            }
        }
        
        _position += alignment;
    }

    public void AppendFormatted(string? str, int alignment)
        => AppendFormatted(str.AsSpan(), alignment);

    private void AlignWritten(int alignment, int startPos, int endPos)
    {
        Debug.Assert(alignment != 0);
        Debug.Assert(endPos >= startPos);

        int len = endPos - startPos;

        // left align (easier)
        if (alignment < 0)
        {
            alignment = -alignment;
            // how much padding do we need to add?
            int padding = alignment - len;
            if (padding == 0)
                return;

            // need to add padding
            if (padding > 0)
            {
                // easy write spaces
                EnsureCapacityFor(padding);
                _charSpan.Slice(endPos, padding).Fill(' ');
                _position += padding;
                return;
            }

            // we wrote too much
            padding = -padding;
            var offset = (startPos + (padding - 1));
            _charSpan[offset] = '…';
            _position = offset + 1;
            return;
        }
        // right align
        else
        {
            // how much padding do we need to add?
            int padding = alignment - len;
            if (padding == 0)
                return;

            // need to add padding
            if (padding > 0)
            {
                // insert spaces
                _charSpan.Slice(startPos, len).CopyTo(_charSpan.Slice(startPos + padding, len));
                _charSpan.Slice(startPos, padding).Fill(' ');
                _position += padding;
                return;
            }

            // we wrote too much
            padding = -padding;
            _charSpan[startPos] = '…';
            var sliceLen = (len - (padding + 1));
            _charSpan.Slice(endPos - sliceLen, sliceLen).CopyTo(_charSpan.Slice(startPos + 1, sliceLen));
            _position = endPos - padding;
            return;
        }
    }

    public void AppendFormatted<T>(T? value, int alignment)
    {
        if (alignment == 0)
            return;

        // we do not know how big value will be once appended
        int startPos = _position;
        AppendFormatted<T>(value);
        int endPos = _position;
        AlignWritten(alignment, startPos, endPos);
    }

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void AppendFormatted<T>(T value, int alignment, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        AppendFormatted(Any.ToString(value), alignment);
    }
#endif

    public void AppendFormatted<T>(T? value, int alignment, string? format)
    {
        if (alignment == 0)
            return;

        // we do not know how big value will be once appended
        int startPos = _position;
        AppendFormatted<T>(value, format);
        int endPos = _position;
        AlignWritten(alignment, startPos, endPos);
    }

#endregion

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = Written.ToString();
        Dispose();
        return str;
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        char[]? toReturn = _rentedCharArray;

        // Defensive clear
        this = default;

        if (toReturn is not null && toReturn.Length > 0)
        {
            ArrayPool<char>.Shared.Return(toReturn, true);
        }
    }

    public override string ToString() => Written.ToString();
}