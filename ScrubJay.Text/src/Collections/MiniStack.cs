namespace ScrubJay.Text.Collections;

internal sealed class MiniStack
{
    private int[] _array;
    private int _count;

    private MiniStack(int[] array, int count)
    {
        _array = array;
        _count = count;
    }
    
    public MiniStack()
    {
        _array = [];
    }

    public void OffsetAll(int offset)
    {
        for (var i = 0; i < _count; i++)
        {
            _array[i] += offset;
        }
    }
    
}