namespace ScrubJay.Memory;

public ref partial struct SpanWriter<T>
{
#region (Try)Write
    public void Write(T item)
    {
        int pos = _position;
        int newPos = pos + 1;
        if (newPos <= _spanLength)
        {
            _span[pos] = item;
            _position = newPos;
            return;
        }

        throw GetWriteException("Write", 1);
    }

    public bool TryWrite(T item)
    {
        int pos = _position;
        int newPos = pos + 1;
        if (newPos <= _spanLength)
        {
            _span[pos] = item;
            _position = newPos;
            return true;
        }
        return false;
    }
#endregion
    
    #region TryWriteAll
    public bool TryWriteAll(params ReadOnlySpan<T> items)
    {
        int pos = _position;
        int newPos = pos + items.Length;
        if (newPos <= _spanLength)
        {
            items.CopyTo(_span.Slice(pos));
            _position = newPos;
            return true;
        }
        return false;
    }
    
    public bool TryWriteAll(T[]? items)
    {
        if (items is null)
            return true;
        
        int pos = _position;
        int newPos = pos + items.Length;
        if (newPos <= _spanLength)
        {
            items.CopyTo(_span.Slice(pos));
            _position = newPos;
            return true;
        }
        return false;
    }
    
    public bool TryWriteAll(IEnumerable<T>? items)
    {
        if (items is null)
            return true;

        int pos = _position;
        var span = _span;

        if (items.TryGetNonEnumeratedCount(out int count))
        {
            int newPos = pos + count;
            if (newPos <= _spanLength)
            {
                foreach (var item in items)
                {
                    span[pos++] = item;
                }
                Debug.Assert(pos == newPos);
                _position = newPos;
                return true;
            }
            return false;
        }

        int start = pos; // if we fail we rollback writes to this point
        
        foreach (var item in items)
        {
            if (!TryWrite(item))
            {
                _position = start;
                return false;
            }
        }
        
        // _position has been updated by all the TryWrites
        return true;
    }
#endregion

#region (Try)WriteAsMany
    public Option<int> TryWriteAsMany(params ReadOnlySpan<T> items)
    {
        int pos = _position;
        int available = _spanLength - pos;
        if (available <= 0)
            return default;

        var toWrite = Math.Min(available, items.Length);
        items.Slice(0, toWrite)
            .CopyTo(_span.Slice(pos));
        return Some(toWrite);
    }
    
    public Option<int> TryWriteAsMany(T[]? items)
    {
        if (items is null)
            return Some(0);
        
        int pos = _position;
        int available = _spanLength - pos;
        if (available <= 0)
            return default;

        var toWrite = Math.Min(available, items.Length);
        items.AsSpan(0, toWrite)
            .CopyTo(_span.Slice(pos));
        return Some(toWrite);
    }
    
    public Option<int> TryWriteAsMany(IEnumerable<T>? items)
    {
        if (items is null)
            return Some(0);

        int count = 0;
        
        foreach (var item in items)
        {
            if (!TryWrite(item))
                break;
            
            count++;
        }
        
        return Some(count);
    }
#endregion
}