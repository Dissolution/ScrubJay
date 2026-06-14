#pragma warning disable IDE0051 // member is unused

namespace ScrubJay.Text.Collections;

internal sealed class MiniStack
{
    private int[] _array;
    private int _count;

    public int Count => _count;

    private MiniStack(int[] array, int count)
    {
        _array = array;
        _count = count;
    }

    public MiniStack()
    {
        _array = [];
    }

    private void GrowBy(int adding)
    {
        var newCapacity = BitOperations.RoundUpToPowerOf2((uint)(_array.Length + adding));
        var newArray = new int[newCapacity];
        if (_count > 0)
        {
            Array.Copy(_array, 0, newArray, 0, _count);
        }
        _array = newArray;
    }

    public void Push(int value)
    {
        if (_count >= _array.Length)
        {
            GrowBy(1);
        }
        _array[_count++] = value;
    }

    public bool TryPeek(out int topValue)
    {
        int i = _count - 1;
        if (i >= 0)
        {
            topValue = _array[i];
            return true;
        }
        topValue = 0;
        return false;
    }

    public bool TryPop(out int topValue)
    {
        int i = _count - 1;
        if (i >= 0)
        {
            topValue = _array[i];
            _count = i;
            return true;
        }
        topValue = 0;
        return false;
    }

    public void OffsetAll(int offset)
    {
        for (var i = 0; i < _count; i++)
        {
            _array[i] += offset;
        }
    }

    public void Clear()
    {
        _count = 0;
    }
}