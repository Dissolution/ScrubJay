#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1716 // Identifiers should not match keywords

using ScrubJay.Unconstrained;

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
}