#pragma warning disable CA1716, CA1062

using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors;

public static partial class Ex
{
    public static ArgRangeException InvalidIndex(int index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        var message = CreateMessage($"Index {index} was not in [0, {available}).", additionalInfo);
        throw new ArgRangeException(Argument.Capture(index, indexName), message);
    }


    public static ArgRangeException InvalidIndex(Index index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        var message = CreateMessage($"Index {index} was not in [0, {available}).", additionalInfo);
        throw new ArgRangeException(Argument.Capture(index, indexName), message);
    }
}

[PublicAPI]
public static partial class Throw
{
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InvalidIndex(int index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        throw Ex.InvalidIndex(index, available, additionalInfo, indexName);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InvalidIndex(Index index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        throw Ex.InvalidIndex(index, available, additionalInfo, indexName);
    }
}

[PublicAPI]
public static partial class Validate
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<int, Exception> Index(int index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index >= (uint)available)
            return Ex.InvalidIndex(index, available, additionalInfo, indexName);
        return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Index, Exception> Index(Index index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset >= (uint)available)
            return Ex.InvalidIndex(index, available, additionalInfo, indexName);
        return index;
    }
}

[PublicAPI]
public static partial class Demand
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Index(int index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index >= (uint)available)
            Throw.InvalidIndex(index, available, additionalInfo, indexName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Index(Index index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset >= (uint)available)
            Throw.InvalidIndex(index, available, additionalInfo, indexName);
    }
}

[PublicAPI]
public static partial class Ensure
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Index(int index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index >= (uint)available)
            Throw.InvalidIndex(index, available, additionalInfo, indexName);
        return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Index(Index index, int available,
        string? additionalInfo = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset >= (uint)available)
            Throw.InvalidIndex(index, available, additionalInfo, indexName);
        return offset;
    }
}