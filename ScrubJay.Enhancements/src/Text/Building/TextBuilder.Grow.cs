using ScrubJay.Enhancements.Text.Pooling;
using ScrubJay.Enhancements.Text.Utilities;

namespace ScrubJay.Enhancements.Text.Building;

public ref partial struct TextBuilder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowBy(int count)
    {
        Debug.Assert(count > 0);
        GrowTo((Capacity + count) * 2);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowTo(int minCapacity)
    {
        Debug.Assert(minCapacity > Capacity || minCapacity == 0);
        char[] array = TextPool.Rent(minCapacity);
        if (_position > 0)
        {
            _charSpan.UnsafeCopyTo(array, _position);
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
        GrowTo(Capacity * 2);
    }
}