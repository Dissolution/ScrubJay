//namespace ScrubJay.Enhancements.Text.Building;
//
//public ref partial struct TextBuilder
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private void GrowBy(int count)
//    {
//        Debug.Assert(count > 0);
//        GrowTo(Capacity + count);
//    }
//
//    [MethodImpl(MethodImplOptions.NoInlining)]
//    private void GrowTo(int minCapacity)
//    {
//        Debug.Assert(minCapacity > Capacity || minCapacity == 0);
//        char[] array = ArrayNest<char>.Rent(minCapacity);
//        if (_position > 0)
//        {
//            Debug.Assert(_charArray is not null);
//            TextHelper.Notsafe.CopyBlock(_charArray!, array, _position);
//        }
//        ArrayNest.Return(_charArray, true);
//        _charArray = array;
//    }
//
//    /// <summary>
//    /// Increases the <see cref="Capacity"/> of this <see cref="TextBuilder"/> to at least twice its current value.
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Grow()
//    {
//        GrowTo(Capacity * 2);
//    }
//}