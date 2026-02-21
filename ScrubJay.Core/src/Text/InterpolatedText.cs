//
//
//#pragma warning disable CA1815, IDE0250, CA1001
//
//namespace ScrubJay.Text;
//
///// <summary>
///// Provides a handler used to append interpolated strings into <see cref="TextBuilder"/> instances.
///// </summary>
///// <remarks>
///// Heavily inspired by <see cref="DefaultInterpolatedStringHandler"/> and System.Text.AppendInterpolatedStringHandler
///// </remarks>
//[PublicAPI]
//[InterpolatedStringHandler]
//[MustDisposeResource(true)]
//public ref struct InterpolatedText : IDisposable
//{
//    private Buffer<char> _buffer;
//
//    public readonly int Length
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _buffer.Count;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public InterpolatedText()
//    {
//        _buffer = new();
//    }
//    
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public InterpolatedText(Buffer<char> buffer)
//    {
//        _buffer = buffer;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public InterpolatedText(int literalLength, int formattedCount)
//    {
//        _buffer = new(literalLength + (formattedCount * 16));
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public InterpolatedText(int literalLength, int formattedCount, Buffer<char> buffer)
//    {
//        _buffer = buffer;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendLiteral(string str)
//    {
//        _buffer.AddMany(str.AsSpan());
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(char ch)
//    {
//        _buffer.Add(ch);
//    }
//    
//    public void AppendFormatted(char ch, int alignment)
//    {
//        if (alignment == 0)
//            return;
//
//        if (alignment < 0)
//        {
//            // left align
//            var span = _buffer.Allocate(-alignment);
//            span[0] = ch;
//            span[1..].Fill(' ');
//        }
//        else
//        {
//            // right align
//            var span = _buffer.Allocate(alignment);
//            span[..^1].Fill(' ');
//            span[^1] = ch;
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(string? str)
//    {
//        _buffer.AddMany(str.AsSpan());
//    }
//
//    public void AppendFormatted(string? str, int alignment)
//        => AppendFormatted(str.AsSpan(), alignment);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted(scoped text text)
//    {
//        _buffer.AddMany(text);
//    }
//
//
//    public void AppendFormatted(scoped text text, int alignment)
//    {
//        if (alignment == 0)
//            return;
//
//
//        if (alignment < 0)
//        {
//            // left align
//            var span = _buffer.Allocate(-alignment);
//            if (text.Length > span.Length)
//            {
//                text[..span.Length].CopyTo(span);
//                return;
//            }
//
//            span[..text.Length].CopyFrom(text);
//            span[text.Length..].Fill(' ');
//        }
//        else
//        {
//            // right align
//            var span = _buffer.Allocate(alignment);
//            if (text.Length > span.Length)
//            {
//                text[^span.Length..].CopyTo(span);
//                return;
//            }
//
//            span[..text.Length].Fill(' ');
//            span[text.Length..].CopyFrom(text);
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AppendFormatted<T>(T? value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        _buffer.AddMany(Any.ToString<T>(value));
//    }
//
//    public void AppendFormatted<T>(T? value, scoped text format)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (format.Length == 0)
//        {
//            AppendFormatted<T>(value);
//        }
//        else if (format.Equate('@'))
//        {
//            // render
//            _buffer.AddMany(value.Render());
//        }
//        else
//        {
//            // no other valid formats?
//            Debugger.Break();
//            throw Ex.NotImplemented();
//        }
//    }
//
//    public void AppendFormatted<T>(T? value, string? format)
//    {
//        if (_builder is null)
//        {
//            if (format.Equate('@'))
//            {
//                // render this value
//                _buffer.AddMany(value.Render());
//            }
//            else if (value is IFormattable)
//            {
//#if NET6_0_OR_GREATER
//                if (value is ISpanFormattable)
//                {
//                    int charsWritten;
//                    while (!((ISpanFormattable)value).TryFormat(_buffer.Available, out charsWritten, format, null))
//                    {
//                        _buffer.Grow();
//                    }
//
//                    _buffer.Count += charsWritten;
//                    return;
//                }
//#endif
//
//                _buffer.AddMany(((IFormattable)value).ToString(format, null));
//            }
//            else if (value is not null)
//            {
//                _buffer.AddMany(value.ToString());
//            }
//        }
//        else
//        {
//            _builder.Format<T>(value, format);
//        }
//    }
//
//#if !NET9_0_OR_GREATER
//    public void AppendFormatted<T>(ReadOnlySpan<T> span)
//    {
//        AppendLiteral(span.ToString());
//    }
//
//    public void AppendFormatted<T>(ReadOnlySpan<T> span, string? format)
//    {
//        if (format.Equate('@'))
//        {
//            AppendLiteral(span.Render());
//        }
//        else
//        {
//            AppendLiteral(span.ToString());
//        }
//    }
//#endif
//
//    [HandlesResourceDisposal]
//    public void Dispose()
//    {
//        if (_builder is null)
//        {
//            _buffer.Dispose();
//        }
//        else
//        {
//            Debug.Assert(_buffer.Count == 0);
//        }
//    }
//
//    [HandlesResourceDisposal]
//    public string ToStringAndDispose()
//    {
//        string str = this.ToString();
//        this.Dispose();
//        return str;
//    }
//
//    public Span<char> AsSpan()
//    {
//        if (_builder is null)
//        {
//            return _buffer.Written;
//        }
//        else
//        {
//            return _builder.Written;
//        }
//    }
//
//    public override string ToString()
//    {
//        if (_builder is null)
//        {
//            return _buffer.Written.AsString();
//        }
//        else
//        {
//            return _builder.ToString();
//        }
//    }
//}