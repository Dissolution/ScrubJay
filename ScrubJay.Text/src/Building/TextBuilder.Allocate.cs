namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public void Allocate(int length, out Span<char> allocated)
    {
        if (length > 0)
        {
            MaybeGrowBy(length);
            allocated = _chars.AsSpan(_position, length);
            TextHelper.Clear(allocated);
            _position += length;
        }
        else
        {
            allocated = [];
        }
    }
    
    public Span<char> Allocate(int length)
    {
        if (length > 0)
        {
            MaybeGrowBy(length);
            var allocated = _chars.AsSpan(_position, length);
            TextHelper.Clear(allocated);
            _position += length;
            return allocated;
        }
        return [];
    }

    public bool TryAllocateAt(int index, int length, out Span<char> allocated)
    {
        if ((uint)index <= (uint)_position)
        {
            if (index == _position)
            {
                Allocate(length, out allocated);
                return true;
            }
            
            if (length <= 0)
            {
                allocated = [];
                return true;
            }

            MaybeGrowBy(length);
            TextHelper.Unsafe.SelfCopy(_chars, index.._position, index + length);
            allocated = _chars.AsSpan(index, length);
            TextHelper.Clear(allocated);
            _position += length;
            return true;
        }
        
        allocated = default;
        return false;
    }

    public bool TryAllocateAt(Range range, out Span<char> allocated)
    {
        int start = range.Start.GetOffset(_position);
        int end = range.End.GetOffset(_position);
        int length = end - start;
        return TryAllocateAt(start, length, out allocated);
    }
}