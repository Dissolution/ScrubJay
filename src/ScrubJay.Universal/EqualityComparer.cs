using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal;

[PublicAPI]
public sealed class AnythingEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static AnythingEqualityComparer<T> Default { get; } = new();


    public bool Equals(T? x, T? y) => Anything.Equate<T>(in x, in y);

    public int GetHashCode(T? value) => Anything.GetHashCode<T>(in value);

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        if (x is T xValue)
        {
            if (y is T yValue)
                return Anything.Equate<T>(in xValue, in yValue);
            return Anything.Equals<T>(in xValue, y);
        }
        else
        {
            if (y is T yValue)
                return Anything.Equals<T>(in yValue, x);
            throw new ArgumentException($"Neither object was a {typeof(T).Alias} value");
        }
    }

    int IEqualityComparer.GetHashCode(object? obj)
    {
        if (obj is T value)
            return GetHashCode(value);
        if (obj is null)
            return 0;
        throw new ArgumentException($"Object is not a {typeof(T).Alias} value", nameof(obj));
    }
}

[PublicAPI]
public sealed class AnythingComparer<T> : IComparer<T>, IComparer
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static AnythingComparer<T> Default { get; } = new();

    public int Compare(T? x, T? y) => Anything.Compare<T>(in x, in y);

    int IComparer.Compare(object? x, object? y)
    {
        if (x is T xValue)
        {
            if (y is T yValue)
            {
                return Anything.Compare<T>(in xValue, in yValue);
            }
            else if (y is null)
            {
                return 1; // everything sorts before null
            }
            else
            {
                throw new ArgumentException($"Object is not a {typeof(T).Alias} value", nameof(y));
            }
        }
        else if (x is null)
        {
            if (y is T yValue)
            {
                return -1; // null sorts before everything
            }
            else if (y is null)
            {
                return 0; // null == null
            }
            else
            {
                throw new ArgumentException($"Object is not a {typeof(T).Alias} value", nameof(y));
            }
        }
        else
        {
            throw new ArgumentException($"Object is not a {typeof(T).Alias} value", nameof(x));
        }
    }
}

[PublicAPI]
public static class EqualityComparer
{
    [OverloadResolutionPriority(100)]
    public static EqualityComparer<T> Default<T>() => EqualityComparer<T>.Default;


#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(90)]
    public static AnythingEqualityComparer<T> Default<T>(
        // ReSharper disable once MethodOverloadWithOptionalParameter
        Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => AnythingEqualityComparer<T>.Default;
#endif
}

[PublicAPI]
public static class Comparer
{
    [OverloadResolutionPriority(100)]
    public static Comparer<T> Default<T>() => Comparer<T>.Default;

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(90)]
    public static AnythingComparer<T> Default<T>(
        // ReSharper disable once MethodOverloadWithOptionalParameter
        Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => AnythingComparer<T>.Default;
#endif
}