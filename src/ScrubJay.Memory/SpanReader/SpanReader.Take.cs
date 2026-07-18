using ScrubJay.Polyfills.Comparison;

namespace ScrubJay.Memory;

public ref partial struct SpanReader<T>
{
#region (Try)Take
    public T Take()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            _position = pos + 1;
            return _span[pos];
        }

        throw GetMoveException("Take", 1);
    }

    public bool TryTake([MaybeNullWhen(false)] out T item)
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            _position = pos + 1;
            item = _span[pos];
            return true;
        }

        item = default;
        return false;
    }

    public Option<T> TryTake()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            _position = pos + 1;
            return Some(_span[pos]);
        }

        return default;
    }
#endregion

#region (Try)TakeMany
    public ReadOnlySpan<T> TakeMany(int count)
    {
        if (count <= 0)
            return default;

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            return _span.Slice(pos, count);
        }

        throw GetMoveException("TakeMany", count);
    }

    public bool TryTakeMany(int count, out ReadOnlySpan<T> taken)
    {
        if (count <= 0)
        {
            taken = default;
            return true;
        }

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            taken = _span.Slice(pos, count);
            return true;
        }

        taken = default;
        return false;
    }

#if NET9_0_OR_GREATER
    public RSOption<ReadOnlySpan<T>> TryTakeMany(int count)
    {
        if (count <= 0)
            return RSOption<ReadOnlySpan<T>>.Some(default);

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            return _span.Slice(pos, count);
        }

        return default;
    }
#endif
#endregion

#region (Try)TakeInto
    public void TakeInto(scoped Span<T> buffer)
    {
        int pos = _position;
        int len = buffer.Length;
        int newPos = pos + len;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            _span.Slice(pos, len).CopyTo(buffer);
            return;
        }

        throw GetMoveException("TakeInto", len);
    }

    public bool TryTakeInto(scoped Span<T> buffer)
    {
        int pos = _position;
        int len = buffer.Length;
        int newPos = pos + len;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            _span.Slice(pos, len).CopyTo(buffer);
            return true;
        }

        return false;
    }
#endregion

#region Take While
#region TakeWhile
    public ReadOnlySpan<T> TakeWhile(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && itemPredicate(span[index]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhile(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemStep(span[index]).IsSome(out step) && step > 0)
        {
            index += step;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhile(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && nextItemsPredicate(span[index..]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhile(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemsStep(span[index..]).IsSome(out step) && step > 0)
        {
            index += step;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhile(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhile(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && prevNextItemsStep(span[..index], span[index..]).IsSome(out step) && step > 0)
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }
#endregion

#region TakeWhileEqualTo
    public ReadOnlySpan<T> TakeWhileEqualTo(T match)
    {
        return TakeWhile((T item) => Relate.Equate(item, match));
    }

    public ReadOnlySpan<T> TakeWhileEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        return TakeWhile((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public ReadOnlySpan<T> TakeWhileEqualTo(scoped ReadOnlySpan<T> match)
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

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
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

        _position = index;
        return span[start..index];
    }

#if NET9_0_OR_GREATER
    public ReadOnlySpan<T> TakeWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
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

        _position = index;
        return span[start..index];
    }
#endif
#endregion

#region TakeWhileEqualToAny
    public ReadOnlySpan<T> TakeWhileEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            return TakeWhile(item => matches.Contains(item));
        }

        return TakeWhile(item => matches.Contains(item, comparer));
    }

    public ReadOnlySpan<T> TakeWhileEqualToAny(
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

        _position = index;
        return span[start..index];
    }
#endregion
#endregion

#region Take Until
#region TakeUntil
    public ReadOnlySpan<T> TakeUntil(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && !itemPredicate(span[index]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntil(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemStep(span[index]).IsSome(out step) || step <= 0))
        {
            index += step;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntil(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !nextItemsPredicate(span[index..]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntil(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemsStep(span[index..]).IsSome(out step) || step <= 0))
        {
            index += step;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntil(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntil(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!prevNextItemsStep(span[..index], span[index..]).IsSome(out step) || step <= 0))
        {
            index++;
        }

        _position = index;
        return span[start..index];
    }
#endregion

#region TakeWhileEqualTo
    public ReadOnlySpan<T> TakeUntilEqualTo(T match)
    {
        return TakeUntil((T item) => Relate.Equate(item, match));
    }

    public ReadOnlySpan<T> TakeUntilEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        return TakeUntil((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public ReadOnlySpan<T> TakeUntilEqualTo(scoped ReadOnlySpan<T> match)
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

        _position = index;
        return span[start..index];
    }

    public ReadOnlySpan<T> TakeUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
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

        _position = index;
        return span[start..index];
    }

#if NET9_0_OR_GREATER
    public ReadOnlySpan<T> TakeUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
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

        _position = index;
        return span[start..index];
    }
#endif
#endregion

#region TakeWhileEqualToAny
    public ReadOnlySpan<T> TakeUntilEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            return TakeUntil(item => matches.Contains(item));
        }

        return TakeUntil(item => matches.Contains(item, comparer));
    }

    public ReadOnlySpan<T> TakeUntilEqualToAny(
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

        _position = index;
        return span[start..index];
    }
#endregion
#endregion
}