using ScrubJay.Universal;

namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Demands
{
    extension<A, T>(A argument)
        where A : struct, IArgument<T>
#if NET9_0_OR_GREATER
        , allows ref struct
        where T : allows ref struct
#endif
    {
        public A IsEqualTo(T? value)
        {
            if (Any.Equals(argument.Value, value))
                return argument;
            throw Ex.Demand<A,T>(argument, $"was not equal to {value}"); 
        }
        
        public A IsNotEqualTo(T? value)
        {
            if (!Any.Equals(argument.Value, value))
                return argument;
            throw Ex.Demand<A,T>(argument, $"was equal to {value}"); 
        }
        
        public A IsEqualTo(T? value, IEqualityComparer<T>? comparer)
        {
            if (comparer is null)
                return IsEqualTo(argument, value);
            if (comparer.Equals(argument.Value!, value!))
                return argument;
            throw Ex.Demand<A,T>(argument, $"was not equal to {value}"); 
        }
        
        public A IsNotEqualTo(T? value, IEqualityComparer<T>? comparer)
        {
            if (comparer is null)
                return IsNotEqualTo(argument, value);
            if (!comparer.Equals(argument.Value!, value!))
                return argument;
            throw Ex.Demand<A,T>(argument, $"was equal to {value}"); 
        }
    }


    extension<A, T>(A argument)
        where A : struct, IArgument<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        public A ReferenceEquals(T? other)
        {
            if (object.ReferenceEquals((object?)argument.Value, (object?)other))
                return argument;
            throw Ex.Demand<A,T>(argument, $"was not the same reference as {other}");
        }
        
    }
}