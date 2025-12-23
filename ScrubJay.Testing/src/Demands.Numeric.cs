namespace ScrubJay.Testing;

partial class Demands
{
#if NET7_0_OR_GREATER
    extension<A, N>(A actual)
        where A : struct, IActual<N>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
        where N : INumberBase<N>
    {
        public A IsZero()
        {
            if (N.IsZero(actual.Value))
                return actual;
            throw new ActualException(actual.Realize(), $"was not zero");
        }
    }

#endif
}