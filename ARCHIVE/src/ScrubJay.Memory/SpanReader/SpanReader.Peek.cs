using ScrubJay.Polyfills.Comparison;

namespace ScrubJay.Memory;

public ref partial struct SpanReader<T>
{
#region (Try)Peek
    public T Peek()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            return _span[pos];
        }

        throw GetMoveException("Peek", 1);
    }

    public bool TryPeek([MaybeNullWhen(false)] out T item)
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            item = _span[pos];
            return true;
        }

        item = default;
        return false;
    }

    public Option<T> TryPeek()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            return Some(_span[pos]);
        }

        return default;
    }
#endregion

#region (Try)PeekMany
    public ReadOnlySpan<T> PeekMany(int count)
    {
        if (count <= 0)
            return default;

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            return _span.Slice(pos, count);
        }

        throw GetMoveException("PeekMany", count);
    }

    public bool TryPeekMany(int count, out ReadOnlySpan<T> peeked)
    {
        if (count <= 0)
        {
            peeked = default;
            return true;
        }

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            peeked = _span.Slice(pos, count);
            return true;
        }

        peeked = default;
        return false;
    }

#if NET9_0_OR_GREATER
    public RSOption<ReadOnlySpan<T>> TryPeekMany(int count)
    {
        if (count <= 0)
            return RSOption<ReadOnlySpan<T>>.Some(default);

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            return _span.Slice(pos, count);
        }

        return default;
    }
#endif
#endregion

#region (Try)PeekInto
    public void PeekInto(scoped Span<T> buffer)
    {
        int pos = _position;
        int len = buffer.Length;
        int newPos = pos + len;
        if (newPos <= _spanLength)
        {
            _span.Slice(pos, len).CopyTo(buffer);
            return;
        }

        throw GetMoveException("PeekInto", len);
    }

    public bool TryPeekInto(scoped Span<T> buffer)
    {
        int pos = _position;
        int len = buffer.Length;
        int newPos = pos + len;
        if (newPos <= _spanLength)
        {
            _span.Slice(pos, len).CopyTo(buffer);
            return true;
        }

        return false;
    }
#endregion

#region Peek While
#region PeekWhile
    public ReadOnlySpan<T> PeekWhile(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && itemPredicate(span[index]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhile(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemStep(span[index]).IsSome(out step) && step > 0)
        {
            index += step;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhile(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && nextItemsPredicate(span[index..]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhile(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemsStep(span[index..]).IsSome(out step) && step > 0)
        {
            index += step;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhile(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhile(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && prevNextItemsStep(span[..index], span[index..]).IsSome(out step) && step > 0)
        {
            index++;
        }
        
        return span[start..index];
    }
#endregion

#region PeekWhileEqualTo
    public ReadOnlySpan<T> PeekWhileEqualTo(T match)
    {
        return PeekWhile((T item) => Relate.Equate(item, match));
    }

    public ReadOnlySpan<T> PeekWhileEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        return PeekWhile((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public ReadOnlySpan<T> PeekWhileEqualTo(scoped ReadOnlySpan<T> match)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match, itemComparer))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }

#if NET9_0_OR_GREATER
    public ReadOnlySpan<T> PeekWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match, comparer))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }
#endif
#endregion

#region PeekWhileEqualToAny
    public ReadOnlySpan<T> PeekWhileEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            return PeekWhile(item => matches.Contains(item));
        }

        return PeekWhile(item => matches.Contains(item, comparer));
    }

    public ReadOnlySpan<T> PeekWhileEqualToAny(
        scoped ReadOnlySpan<T> matches,
        IEqualityComparer<T>? itemComparer = null)
    {
        var (span, len, start) = this;
        int index = start;
        itemComparer ??= EqualityComparer<T>.Default;

        while (index < len && matches.Contains(span[index], itemComparer))
        {
            index++;
        }
        
        return span[start..index];
    }
#endregion
#endregion

#region Peek Until
#region PeekUntil
    public ReadOnlySpan<T> PeekUntil(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && !itemPredicate(span[index]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntil(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemStep(span[index]).IsSome(out step) || step <= 0))
        {
            index += step;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntil(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !nextItemsPredicate(span[index..]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntil(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemsStep(span[index..]).IsSome(out step) || step <= 0))
        {
            index += step;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntil(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntil(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!prevNextItemsStep(span[..index], span[index..]).IsSome(out step) || step <= 0))
        {
            index++;
        }
        
        return span[start..index];
    }
#endregion

#region PeekWhileEqualTo
    public ReadOnlySpan<T> PeekUntilEqualTo(T match)
    {
        return PeekUntil((T item) => Relate.Equate(item, match));
    }

    public ReadOnlySpan<T> PeekUntilEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        return PeekUntil((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public ReadOnlySpan<T> PeekUntilEqualTo(scoped ReadOnlySpan<T> match)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }

    public ReadOnlySpan<T> PeekUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match, itemComparer))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }

#if NET9_0_OR_GREATER
    public ReadOnlySpan<T> PeekUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return [];

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match, comparer))
        {
            index += matchLen;
        }
        
        return span[start..index];
    }
#endif
#endregion

#region PeekWhileEqualToAny
    public ReadOnlySpan<T> PeekUntilEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            return PeekUntil(item => matches.Contains(item));
        }

        return PeekUntil(item => matches.Contains(item, comparer));
    }

    public ReadOnlySpan<T> PeekUntilEqualToAny(
        scoped ReadOnlySpan<T> matches,
        IEqualityComparer<T>? itemComparer = null)
    {
        var (span, len, start) = this;
        int index = start;
        itemComparer ??= EqualityComparer<T>.Default;

        while (index < len && !matches.Contains(span[index], itemComparer))
        {
            index++;
        }
        
        return span[start..index];
    }
#endregion
#endregion
}