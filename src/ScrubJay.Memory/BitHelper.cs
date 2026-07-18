namespace ScrubJay.Memory;

public static partial class BitHelper
{
    public static class Notsafe
    {

    }

    public static bool TrySelfCopy<U>(U[]? array, int sourceIndex, int destIndex, int count)
        where U : unmanaged
    {
        if (array is null) return false;
        uint available = (uint)array.Length - (uint)count;
        if ((uint)sourceIndex > available || (uint)destIndex > available) return false;
        Unsafe.SelfCopy(array, sourceIndex, destIndex, count);
        return true;
    }
    
    public static bool TrySelfCopy<U>(U[]? array, Range sourceRange, int destIndex)
        where U : unmanaged
    {
        if (array is null) return false;
        var (sourceIndex, count) = sourceRange.GetOffsetAndLength(array.Length);
        uint available = (uint)array.Length - (uint)count;
        if ((uint)sourceIndex > available || (uint)destIndex > available) return false;
        Unsafe.SelfCopy(array, sourceIndex, destIndex, count);
        return true;
    }
    
    public static bool TrySelfCopy<U>(U[]? array, int sourceIndex, Range destRange)
        where U : unmanaged
    {
        if (array is null) return false;
        var (destIndex, count) = destRange.GetOffsetAndLength(array.Length);
        uint available = (uint)array.Length - (uint)count;
        if ((uint)sourceIndex > available || (uint)destIndex > available) return false;
        Unsafe.SelfCopy(array, sourceIndex, destIndex, count);
        return true;
    }
    
    public static bool TrySelfCopy<U>(U[]? array, Range sourceRange, Range destRange)
        where U : unmanaged
    {
        if (array is null) return false;
        var (sourceIndex, sourceCount) = sourceRange.GetOffsetAndLength(array.Length);
        var (destIndex, destCount) = destRange.GetOffsetAndLength(array.Length);
        if (destCount != sourceCount) return false;
        uint available = (uint)array.Length - (uint)sourceCount;
        if ((uint)sourceIndex > available || (uint)destIndex > available) return false;
        Unsafe.SelfCopy(array, sourceIndex, destIndex, destCount);
        return true;
    }
}