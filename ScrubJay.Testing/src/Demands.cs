using ScrubJay.Universal;

namespace ScrubJay.Testing;

[StackTraceHidden]
public static partial class Demands
{
    extension<A, T>(A actual)
        where A : struct, IActual<T>
#if NET9_0_OR_GREATER
        , allows ref struct
        where T : allows ref struct
#endif
    {
        public A IsEqualTo(T? other)
        {
            if (Any.Equals(actual.Value, other))
                return actual;
            throw new ActualException(actual.Realize(), $"was not equal to {other}");
        }
    }

    public static A ReferenceEquals<A, T>(this A actual, T? other)
        where A : struct, IActual<T?>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (object.ReferenceEquals((object?)actual.Value, (object?)other))
            return actual;
        throw new ActualException(actual.Realize(), $"was not the same reference as {other}");
    }
}