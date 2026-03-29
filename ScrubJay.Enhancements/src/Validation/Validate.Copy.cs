namespace ScrubJay.Enhancements.Validation;

public static partial class Validate
{
    public static Result<int> CopyTo(int count, int available)
    {
        if ((uint)count < (uint)available)
            return count;
        return Ex.ArgRange(count, 0..available);
    }
    
    public static Result<int> CopyTo<T>(int count, scoped Span<T> destination)
    {
        if ((uint)count < (uint)destination.Length)
            return count;
        return Ex.ArgRange(count, 0..destination.Length);
    }
}