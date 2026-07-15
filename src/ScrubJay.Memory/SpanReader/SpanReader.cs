using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Memory;

[PublicAPI]
[DebuggerDisplay("{Display,nq}")]
[StructLayout(LayoutKind.Auto)]
public ref partial struct SpanReader<T>
{
    internal readonly ReadOnlySpan<T> _span;
    internal readonly int _spanLength;
    internal int _position;
    
    public int Position
    {
        readonly get => _position;
        set => TrySetPosition(value);
    }

    public readonly bool IsCompleted => _position >= _spanLength;

    public readonly int RemainingLength => _spanLength - _position;

    public readonly ReadOnlySpan<T> Previous => _span[.._position];

    public readonly ReadOnlySpan<T> Next => _span[_position..];

    public SpanReader(ReadOnlySpan<T> span)
    {
        _span = span;
        _spanLength = span.Length;
        _position = 0;
    }

    internal void Deconstruct(out ReadOnlySpan<T> span, out int spanLength, out int position)
    {
        span = _span;
        spanLength = _spanLength;
        position = _position;
    }

    private readonly InvalidOperationException GetMoveException(
        string methodName,
        int count)
    {
        using InterpolatedText message = $"SpanReader<{TypeName.For<T>()}> cannot {methodName} {count} items: ";
        int remains = RemainingLength;
        if (remains == 0)
        {
            message.Write("no items left to read");
        }
        else
        {
            message.Write($"only {remains} items left to read");
        }
        return new InvalidOperationException(message.ToString());
    }

//
//#region Peek!
//#region Peek / Try / Into
//    public readonly bool TryPeek([MaybeNullWhen(false)] out T item)
//    {
//        if (_position < _spanLength)
//        {
//            item = _span[_position];
//            return true;
//        }
//        item = default;
//        return false;
//    }
//
//    public readonly T Peek()
//    {
//        if (_position < _spanLength)
//            return _span[_position];
//        throw GetPositionMoveException();
//    }
//
//
//    public readonly bool TryPeekMany(int count, out ReadOnlySpan<T> slice)
//    {
//        if (count <= 0)
//        {
//            slice = ReadOnlySpan<T>.Empty;
//            return true;
//        }
//
//        if (_position + count <= _spanLength)
//        {
//            slice = _span.Slice(_position, count);
//            return true;
//        }
//
//        slice = [];
//        return false;
//    }
//
//    public readonly ReadOnlySpan<T> PeekMany(int count)
//    {
//        //Guard.IsGrequalTo(count, 0);
//        if (_position + count <= _spanLength)
//            return _span.Slice(_position, count);
//
//        throw GetPositionMoveException();
//    }
//
//    public readonly T[] PeekToArray(int count)
//    {
//        //Guard.IsGrequalTo(count, 0);
//        if (_position + count <= _spanLength)
//            return _span.Slice(_position, count).ToArray();
//
//        throw GetPositionMoveException();
//    }
//
//
//    public readonly bool TryPeekInto(Span<T> destination)
//    {
//        int count = destination.Length;
//        if (_position + count <= _spanLength)
//        {
//            _span.Slice(_position, count).CopyTo(destination);
//            return true;
//        }
//
//        return false;
//    }
//
//    public readonly void PeekInto(Span<T> destination)
//    {
//        int count = destination.Length;
//        if (_position + count <= _spanLength)
//        {
//            _span.Slice(_position, count).CopyTo(destination);
//            return;
//        }
//
//        throw GetPositionMoveException();
//    }
//#endregion
//
//#region PeekWhile
//    public readonly ReadOnlySpan<T> PeekWhile(Func<T, bool> itemPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && itemPredicate(span[index]))
//            index++;
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekWhile(ReadOnlySpanPredicate<T> spanPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && spanPredicate(span[index..]))
//            index++;
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekWhile(ScanPredicate<T> prevNextPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && prevNextPredicate(span[..index], span[index..]))
//            index++;
//        return span[start..index];
//    }
//#endregion
//
//#region PeekWhileMatching
//    public readonly ReadOnlySpan<T> PeekWhileMatching(
//        T match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//            return PeekWhile(item => EqualityComparer<T>.Default.Equals(item, match));
//        return PeekWhile(item => comparer.Equals(item, match));
//    }
//
//    public readonly ReadOnlySpan<T> PeekWhileMatching(
//        scoped ReadOnlySpan<T> match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        int matchLen = match.Length;
//        if (matchLen == 0)
//            return [];
//
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && span.Slice(index, matchLen).SequenceEqual(match, comparer))
//        {
//            index += matchLen;
//        }
//
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekWhileMatchingAny(
//        ICollection<T> matches,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            return PeekWhile(item => matches.Contains(item));
//        }
//
//        return PeekWhile(item => matches.Contains(item, comparer));
//    }
//#endregion
//
//#region PeekUntil
//    public readonly ReadOnlySpan<T> PeekUntil(Func<T, bool> itemPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !itemPredicate(span[index]))
//            index++;
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekUntil(ReadOnlySpanPredicate<T> spanPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !spanPredicate(span[index..]))
//            index++;
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekUntil(ScanPredicate<T> prevNextPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !prevNextPredicate(span[..index], span[index..]))
//            index++;
//        return span[start..index];
//    }
//#endregion
//
//#region PeekUntilMatching
//    public readonly ReadOnlySpan<T> PeekUntilMatching(
//        T match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//            return PeekUntil(item => !EqualityComparer<T>.Default.Equals(item, match));
//        return PeekUntil(item => !comparer.Equals(item, match));
//    }
//
//    public readonly ReadOnlySpan<T> PeekUntilMatching(
//        scoped ReadOnlySpan<T> match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        int matchLen = match.Length;
//        if (matchLen == 0)
//            return [];
//
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !span.Slice(index, matchLen).SequenceEqual(match, comparer))
//        {
//            index++;
//        }
//
//        return span[start..index];
//    }
//
//    public readonly ReadOnlySpan<T> PeekUntilMatchingAny(
//        ICollection<T> matches,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            return PeekUntil(item => !matches.Contains(item));
//        }
//
//        return PeekUntil(item => !matches.Contains(item, comparer));
//    }
//#endregion
//#endregion /Peek!
//#region Skip
//#region (Try)Skip
//    public bool TrySkip()
//    {
//        if (_position < _spanLength)
//        {
//            _position++;
//            return true;
//        }
//
//        return false;
//    }
//
//    public void Skip()
//    {
//        if (_position < _spanLength)
//        {
//            _position++;
//            return;
//        }
//
//        throw GetPositionMoveException();
//    }
//
//    public bool TrySkip(int count)
//    {
//        if (count <= 0)
//            return true;
//
//        if (_position + count <= _spanLength)
//        {
//            _position += count;
//            return true;
//        }
//
//        return false;
//    }
//
//
//    public void Skip(int count)
//    {
//        //Guard.IsGrequalTo(count, 0);
//
//        if (_position + count <= _spanLength)
//        {
//            _position += count;
//            return;
//        }
//
//        throw GetPositionMoveException();
//    }
//#endregion
//
//#region SkipWhile
//    public void SkipWhile(Func<T, bool> itemPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && itemPredicate(span[index]))
//            index++;
//        _position = index;
//    }
//
//    public void SkipWhile(ReadOnlySpanPredicate<T> spanPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && spanPredicate(span[index..]))
//            index++;
//        _position = index;
//    }
//
//    public void SkipWhile(ScanPredicate<T> prevNextPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && prevNextPredicate(span[..index], span[index..]))
//            index++;
//        _position = index;
//    }
//#endregion
//
//#region SkipWhileMatching
//    public void SkipWhileMatching(
//        T match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            SkipWhile(item => EqualityComparer<T>.Default.Equals(item, match));
//        }
//        else
//        {
//            SkipWhile(item => comparer.Equals(item, match));
//        }
//    }
//
//    public void SkipWhileMatching(
//        scoped ReadOnlySpan<T> match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        int matchLen = match.Length;
//        if (matchLen == 0)
//            return;
//
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && span.Slice(index, matchLen).SequenceEqual(match, comparer))
//        {
//            index += matchLen;
//        }
//
//        _position = index;
//    }
//
//    public void SkipWhileMatchingAny(
//        ICollection<T> matches,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            SkipWhile(item => matches.Contains(item));
//        }
//        else
//        {
//            SkipWhile(item => matches.Contains(item, comparer));
//        }
//    }
//#endregion
//
//#region SkipUntil
//    public void SkipUntil(Func<T, bool> itemPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !itemPredicate(span[index]))
//            index++;
//        _position = index;
//    }
//
//    public void SkipUntil(ReadOnlySpanPredicate<T> spanPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !spanPredicate(span[index..]))
//            index++;
//        _position = index;
//    }
//
//    public void SkipUntil(ScanPredicate<T> prevNextPredicate)
//    {
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !prevNextPredicate(span[..index], span[index..]))
//            index++;
//        _position = index;
//    }
//#endregion
//
//#region SkipUntilMatching
//    public void SkipUntilMatching(
//        T match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            SkipUntil(item => !EqualityComparer<T>.Default.Equals(item, match));
//        }
//        else
//        {
//            SkipUntil(item => !comparer.Equals(item, match));
//        }
//    }
//
//    public void SkipUntilMatching(
//        scoped ReadOnlySpan<T> match,
//        IEqualityComparer<T>? comparer = null)
//    {
//        int matchLen = match.Length;
//        if (matchLen == 0)
//            return;
//
//        var span = _span;
//        int start = _position;
//        int index = start;
//        int len = _spanLength;
//
//        while (index < len && !span.Slice(index, matchLen).SequenceEqual(match, comparer))
//        {
//            index++;
//        }
//
//        _position = index;
//    }
//
//    public void SkipUntilMatchingAny(
//        ICollection<T> matches,
//        IEqualityComparer<T>? comparer = null)
//    {
//        if (comparer is null)
//        {
//            SkipUntil(item => !matches.Contains(item));
//        }
//        else
//        {
//            SkipUntil(item => !matches.Contains(item, comparer));
//        }
//    }
//#endregion
//#endregion

    public bool TrySetPosition(int index)
    {
        if ((uint)index <= (uint)_spanLength)
        {
            _position = index;
            return true;
        }
        return false;
    }
    
    public bool TrySetPosition(Index index)
    {
        int offset = index.GetOffset(_spanLength);
        if ((uint)offset <= (uint)_spanLength)
        {
            _position = offset;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Resets this <see cref="SpanReader{T}"/> back to its beginning position
    /// </summary>
    public void Reset() => _position = 0;

    private readonly string Display()
    {
        var builder = new DefaultInterpolatedStringHandler();

        // special handling for string-types
        if (typeof(T) == typeof(char))
        {
            // up to previous 16 chars
            var previousStart = _position - 16;
            if (previousStart <= 0)
            {
                // hard clamp
                previousStart = 0;
            }
            else
            {
                // there are previous characters we are not showing
                builder.AppendLiteral("… ");
            }
            builder.AppendFormatted(_span[previousStart.._position].ToString());

            // we are here
            builder.AppendLiteral(" ⌖ ");

            // up to 16 next chars
            var nextEnd = _position + 16;
            if (nextEnd >= _spanLength)
            {
                // all the rest
                builder.AppendFormatted(_span[_position..].ToString());
            }
            else
            {
                builder.AppendFormatted(_span[_position..nextEnd].ToString());
                // there were more
                builder.AppendLiteral(" …");
            }
        }
        else
        {
            // up to previous 4 items
            var previousStart = _position - 4;
            if (previousStart <= 0)
            {
                // hard clamp
                previousStart = 0;
            }
            else
            {
                // there are previous items we are not showing
                builder.AppendLiteral("… ");
            }

            for (var i = previousStart; i < _position; i++)
            {
                builder.AppendFormatted(_span[i]);
                builder.AppendLiteral(", ");
            }

            // we are here
            builder.AppendLiteral("⌖ ");

            // up to 4 next items
            var incEnd = _spanLength - 1;
            var nextEnd = _position + 4;
            if (nextEnd >= _spanLength)
            {
                // all the rest
                for (var i = _position; i < incEnd; i++)
                {
                    builder.AppendFormatted(_span[i]);
                    builder.AppendLiteral(", ");
                }
                builder.AppendFormatted(_span[incEnd]);
            }
            else
            {
                for (var i = _position; i < nextEnd; i++)
                {
                    builder.AppendFormatted(_span[i]);
                    builder.AppendLiteral(", ");
                }
                builder.AppendFormatted(_span[nextEnd]);

                // there were more
                builder.AppendLiteral(" …");
            }

        }
        return builder.ToStringAndClear();
    }

    public override string ToString() => $"[0..{_position}..{_spanLength})";
}