using ScrubJay.Polyfills.Comparison;

namespace ScrubJay.Memory;

public ref partial struct SpanReader<T>
{
#region (Try)Skip
    public void Skip()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            _position = pos + 1;
            return;
        }

        throw GetMoveException("Skip", 1);
    }

    public bool TrySkip()
    {
        int pos = _position;
        if (pos < _spanLength)
        {
            _position = pos + 1;
            return true;
        }

        return false;
    }
#endregion

#region (Try)SkipMany
    public void SkipMany(int count)
    {
        if (count <= 0)
            return;

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            return;
        }

        throw GetMoveException("SkipMany", count);
    }

    public bool TrySkipMany(int count)
    {
        if (count <= 0)
        {
            return true;
        }

        int pos = _position;
        int newPos = pos + count;
        if (newPos <= _spanLength)
        {
            _position = newPos;
            return true;
        }

        return false;
    }
#endregion

#region Skip While
#region SkipWhile
    public void SkipWhile(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && itemPredicate(span[index]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipWhile(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemStep(span[index]).IsSome(out step) && step > 0)
        {
            index += step;
        }

        _position = index;
    }

    public void SkipWhile(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && nextItemsPredicate(span[index..]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipWhile(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && nextItemsStep(span[index..]).IsSome(out step) && step > 0)
        {
            index += step;
        }

        _position = index;
    }

    public void SkipWhile(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipWhile(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && prevNextItemsStep(span[..index], span[index..]).IsSome(out step) && step > 0)
        {
            index++;
        }

        _position = index;
    }
#endregion

#region SkipWhileEqualTo
    public void SkipWhileEqualTo(T match)
    {
        SkipWhile((T item) => Relate.Equate(item, match));
    }

    public void SkipWhileEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        SkipWhile((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public void SkipWhileEqualTo(scoped ReadOnlySpan<T> match)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match))
        {
            index += matchLen;
        }

        _position = index;
    }

    public void SkipWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match, itemComparer))
        {
            index += matchLen;
        }

        _position = index;
    }

#if NET9_0_OR_GREATER
    public void SkipWhileEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && Relate.Equate(span.Slice(index, matchLen), match, comparer))
        {
            index += matchLen;
        }

        _position = index;
    }
#endif
#endregion

#region SkipWhileEqualToAny
    public void SkipWhileEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            SkipWhile(item => matches.Contains(item));
        }
        else
        {
            SkipWhile(item => matches.Contains(item, comparer));
        }
    }

    public void SkipWhileEqualToAny(
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
    }
#endregion
#endregion

#region Skip Until
#region SkipUntil
    public void SkipUntil(Func<T, bool> itemPredicate)
    {
        var (span, len, start) = this;
        int index = start;
        while (index < len && !itemPredicate(span[index]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipUntil(NextItemStep<T> nextItemStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemStep(span[index]).IsSome(out step) || step <= 0))
        {
            index += step;
        }

        _position = index;
    }

    public void SkipUntil(NextItemsPredicate<T> nextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !nextItemsPredicate(span[index..]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipUntil(NextItemsStep<T> nextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!nextItemsStep(span[index..]).IsSome(out step) || step <= 0))
        {
            index += step;
        }

        _position = index;
    }

    public void SkipUntil(PrevNextItemsPredicate<T> prevNextItemsPredicate)
    {
        var (span, len, start) = this;
        int index = start;

        while (index < len && !prevNextItemsPredicate(span[..index], span[index..]))
        {
            index++;
        }

        _position = index;
    }

    public void SkipUntil(PrevNextItemsStep<T> prevNextItemsStep)
    {
        var (span, len, start) = this;
        int index = start;
        int step;

        while (index < len && (!prevNextItemsStep(span[..index], span[index..]).IsSome(out step) || step <= 0))
        {
            index++;
        }

        _position = index;
    }
#endregion

#region SkipWhileEqualTo
    public void SkipUntilEqualTo(T match)
    {
        SkipUntil((T item) => Relate.Equate(item, match));
    }

    public void SkipUntilEqualTo(
        T match,
        IEqualityComparer<T>? comparer)
    {
        SkipUntil((T item) => Relate.Equate<T>(item, match, comparer));
    }

    public void SkipUntilEqualTo(scoped ReadOnlySpan<T> match)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match))
        {
            index += matchLen;
        }

        _position = index;
    }

    public void SkipUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<T>? itemComparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match, itemComparer))
        {
            index += matchLen;
        }

        _position = index;
    }

#if NET9_0_OR_GREATER
    public void SkipUntilEqualTo(scoped ReadOnlySpan<T> match, IEqualityComparer<ReadOnlySpan<T>>? comparer)
    {
        int matchLen = match.Length;
        if (matchLen == 0)
            return;

        var (span, len, start) = this;
        int index = start;

        while (index < len && !Relate.Equate(span.Slice(index, matchLen), match, comparer))
        {
            index += matchLen;
        }

        _position = index;
    }
#endif
#endregion

#region SkipWhileEqualToAny
    public void SkipUntilEqualToAny(
        ICollection<T> matches,
        IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            SkipUntil(item => matches.Contains(item));
        }
        else
        {
            SkipUntil(item => matches.Contains(item, comparer));
        }
    }

    public void SkipUntilEqualToAny(
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
    }
#endregion
#endregion
}