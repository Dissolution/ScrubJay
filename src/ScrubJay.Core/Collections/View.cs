#pragma warning disable S3218

using System.Collections.Immutable;

namespace ScrubJay.Collections;

[PublicAPI]
public static class View
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "array")]
    private static extern ref T[]? GetRawArray<T>(ref ImmutableArray<T> immutableArray);

    public static View<T> Create<T>(T item)
    {
        return new View<T>([item]);
    }

    public static View<T> Create<T>(T item1, T item2)
    {
        return new View<T>([item1, item2]);
    }

    public static View<T> Create<T>(T item1, T item2, T item3)
    {
        return new View<T>([item1, item2, item3]);
    }

    [OverloadResolutionPriority(100)]
    public static View<T> Create<T>(ref ImmutableArray<T> immutableArray)
    {
        T[]? array = GetRawArray(ref immutableArray);
        if (array is null)
            return View<T>.Uninitialized;
        return new View<T>(array);
    }

    [OverloadResolutionPriority(90)]
    public static View<T> Create<T>(T[]? array)
    {
        if (array is null)
            return View<T>.Uninitialized;
        return new View<T>(array);
    }

    [OverloadResolutionPriority(80)]
    public static View<T> Create<T>(params ReadOnlySpan<T> values)
    {
        if (values.IsEmpty)
            return View<T>.Empty;
        return new(values.ToArray());
    }

    [OverloadResolutionPriority(50)]
    public static View<T> Create<T>(IEnumerable<T>? items)
    {
        if (items is null)
            return View<T>.Uninitialized;
        return new(items.ToArray());
    }


    public static LimitedViewBuilder<T> Build<T>(int capacity) => new(capacity);

    public static ViewBuilder<T> Build<T>() => new();
}