using ScrubJay.Text.Pooling;
using ScrubJay.Text.Utilities;

namespace ScrubJay.Text.Building;

public ref partial struct TextBuilder
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowImpl(int minCapacity)
    {
        Debug.Assert(minCapacity > Capacity || minCapacity == 0);
        char[] array = TextPool.Rent(minCapacity);
        if (_position > 0)
        {
            Debug.Assert(_position <= array.Length);
            TextHelper.Unsafe.CopyTo(_charSpan, array, _position);
        }
        TextPool.Return(_charArray);
        _charSpan = _charArray = array;
    }

    /// <summary>
    /// Increases the <see cref="Capacity"/> of this <see cref="TextBuilder"/> to at least twice its current value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grow()
    {
        GrowImpl(Capacity * 2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GrowBy(int count)
    {
        if (count > 0)
        {
            GrowImpl((Capacity + count) * 2);
        }
    }
}