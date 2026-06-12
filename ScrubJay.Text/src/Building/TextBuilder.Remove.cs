namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    #region Remove At (index, range)
    public bool TryRemoveAt(int index)
    {
        if ((uint)index < (uint)_position)
        {
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, index, 1);
            _position--;
            return true;
        }
        return false;
    }

    public bool TryRemoveAt(int index, out char removed)
    {
        if ((uint)index < (uint)_position)
        {
            removed = _chars[index];
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, index, 1);
            _position--;
            return true;
        }
        removed = default;
        return false;
    }

    public bool TryRemoveAt(Index index)
    {
        if (index.TryGetOffset(_position, out var offset))
        {
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, offset, 1);
            _position--;
            return true;
        }
        return false;
    }

    public bool TryRemoveAt(Index index, out char removed)
    {
        if (index.TryGetOffset(_position, out var offset))
        {
            removed = _chars[offset];
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, offset, 1);
            _position--;
            return true;
        }
        removed = default;
        return false;
    }

    public bool TryRemoveAt(Range range)
    {
        if (range.TryGetOffsetAndLength(_position, out int offset, out int length))
        {
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, offset, length);
            _position -= length;
            return true;
        }
        return false;
    }

    public bool TryRemoveAt(Range range, [NotNullWhen(true)] out string? removed)
    {
        if (range.TryGetOffsetAndLength(_position, out int offset, out int length))
        {
            removed = new string(_chars, offset, length);
            TextHelper.Unsafe.ShiftItemsLeft(_chars, _position, offset, length);
            _position -= length;
            return true;
        }
        removed = null;
        return false;
    }
    #endregion


    public int RemoveWhere(Func<char, bool>? charPredicate)
    {
        if (charPredicate is null)
            return 0;

        int freeIndex = 0; // the first free slot in span
        int pos = _position;
        var span = Written;

        // Find the first item which needs to be removed.
        while ((freeIndex < pos) && !charPredicate(span[freeIndex]))
            freeIndex++;

        if (freeIndex >= pos)
            return 0;

        int current = freeIndex + 1;
        while (current < pos)
        {
            // Find the first item which needs to be kept.
            while ((current < pos) && charPredicate(span[current]))
                current++;

            if (current < pos)
            {
                // copy item to the free slot
                span[freeIndex++] = span[current++];
            }
        }

        int removedCount = pos - freeIndex;
        _position = freeIndex;
        return removedCount;
    }

    public bool TryRemoveLast(int count)
    {
        if (count <= _position)
        {
            _position -= count;
            return true;
        }
        return false;
    }


    public TextBuilder TrimEnd()
    {
        int end = _position - 1;
        for (; end >= 0; end--)
        {
            if (!char.IsWhiteSpace(_chars[end]))
            {
                break;
            }
        }

        _position = end + 1;
        return this;
    }


    public TextBuilder Clear()
    {
        _position = 0;
        return this;
    }
}