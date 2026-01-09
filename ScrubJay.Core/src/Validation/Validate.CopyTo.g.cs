#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<int> CanCopyTo<T>(int count, T[]? destArray, int destArrayIndex = 0,
        [CallerArgumentExpression(nameof(count))]
        string? countName = null,
        [CallerArgumentExpression(nameof(destArray))]
        string? destArrayName = null)
    {
        if (count <= 0)
            return count;

        if (destArray is null)
            return Ex.ArgNull(destArrayName);

        if ((uint)destArrayIndex + (uint)count <= (uint)destArray.Length)
            return count;

        return Ex.ArgRange(count, $"can not copy {count} items to [{destArrayIndex}..{destArray.Length}]", countName);
    }

    public static Result<int> CanCopyTo<T>(int count, Span<T> destination,
        [CallerArgumentExpression(nameof(count))]
        string? countName = null,
        [CallerArgumentExpression(nameof(destination))]
        string? destinationName = null)
    {
        if (count <= 0)
            return count;

        if ((uint)count <= (uint)destination.Length)
            return count;

        return Ex.ArgRange(count, $"can not copy {count} items to [{destination.Length}]", countName);
    }

    public static Result<int> CanCopyTo(int count, Array? destArray, int destArrayIndex = 0,
        [CallerArgumentExpression(nameof(count))]
        string? countName = null,
        [CallerArgumentExpression(nameof(destArray))]
        string? destArrayName = null)
    {
        if (count <= 0)
            return count;

        if (destArray is null)
            return Ex.ArgNull(destArrayName);

        if ((uint)destArrayIndex + (uint)count <= (uint)destArray.Length)
            return count;

        return Ex.ArgRange(count, $"can not copy {count} items to [{destArrayIndex}..{destArray.Length}]", countName);
    }
}
