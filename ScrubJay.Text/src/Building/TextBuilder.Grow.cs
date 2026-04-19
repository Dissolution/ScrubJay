namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowTo(int minCapacity)
    {
        Debug.Assert(minCapacity > Capacity || minCapacity == 0);
        char[] array = TextPool.Rent(minCapacity);
        if (_position > 0)
        {
            Debug.Assert(_chars is not null);
            TextHelper.Unsafe.CopyTo(_chars!, array, _position);
        }
        TextPool.Return(array);
        _chars = array;
    }

    private void MaybeGrowBy(int count)
    {
        Debug.Assert(count >= 0);
        if (_position + count > _chars.Length)
        {
            GrowBy(count);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GrowBy(int count)
    {
        if (count > 0)
        {
            GrowTo(Capacity + count);
        }
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