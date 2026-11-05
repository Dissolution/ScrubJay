#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1716 // Identifiers should not match keywords


namespace ScrubJay.Functional;

[PublicAPI]
public static partial class Option
{
    [OverloadResolutionPriority(100)]
    public static Option<S> NotNull<S>(S? nullable, Constrain.IsStruct<S> _ = default)
        where S : struct
    {
        if (nullable.HasValue)
            return nullable.GetValueOrDefault();
        return default;
    }

    [OverloadResolutionPriority(75)]
    public static Option<C> NotNull<C>([AllowNull] C? value, Constrain.IsClass<C> _ = default)
        where C : class
    {
        if (value is null)
            return default;
        return value;
    }

    [OverloadResolutionPriority(50)]
    public static Option<T> NotNull<T>(T? value, Constrain.Unbounded<T> _ = default)
    {
        if (value is null)
            return default;
        return value;
    }

    public delegate bool TryInvoke<R>(out R result);

    public delegate bool TryInvoke<A1, R>(A1 arg1, out R result);

    public delegate bool TryInvoke<A1, A2, R>(A1 arg1, A2 arg2, out R result);

    public delegate bool TryInvoke<A1, A2, A3, R>(A1 arg1, A2 arg2, A3 arg3, out R result);

    public static Option<R> Invoke<R>(TryInvoke<R> tryInvoke)
    {
        if (tryInvoke(out var result))
        {
            return result;
        }
        return default;
    }

    public static Option<R> Invoke<A1, R>(A1 arg1, TryInvoke<A1, R> tryInvoke)
    {
        if (tryInvoke(arg1, out var result))
        {
            return result;
        }
        return default;
    }

    public static Option<R> Invoke<A1, A2, R>(A1 arg1, A2 arg2, TryInvoke<A1, A2, R> tryInvoke)
    {
        if (tryInvoke(arg1, arg2, out var result))
        {
            return result;
        }
        return default;
    }

    public static Option<R> Invoke<A1, A2, A3, R>(A1 arg1, A2 arg2, A3 arg3, TryInvoke<A1, A2, A3, R> tryInvoke)
    {
        if (tryInvoke(arg1, arg2, arg3, out var result))
        {
            return result;
        }
        return default;
    }
}