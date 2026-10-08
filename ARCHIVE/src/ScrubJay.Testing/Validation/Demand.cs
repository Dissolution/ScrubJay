using System.Linq.Expressions;
using ScrubJay.Errors;
using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;
using ScrubJay.Universal;

namespace ScrubJay.Testing.Validation;


[PublicAPI]
public static partial class Demand
{
    [AssertionMethod]
    public static void True(
        [AssertionCondition(AssertionConditionType.IS_TRUE)]
        Expression<Func<bool>> expression)
    {
        var compiledPredicate = expression.Compile();
        if (compiledPredicate())
            return;
        throw Ex.Arg(expression, "was not true");
    }

//    [AssertionMethod]
//    public static void Equal<T>(Expression<Func<T>> left, Expression<Func<T>> right)
//    {
//        
//    }


    [AssertionMethod]
    public static void AllTrue<T>(Expression<Func<T, bool>> predicate, params ReadOnlySpan<T> values)
    {

    }
}

public static partial class Demand
{
    public static Argument<T> That<T>(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return Argument<T>.Capture(in argument, argumentName);
    }
}

[PublicAPI]
public static class ArgumentExtensions
{
    extension<T>(in Argument<T> argument)
    {
        public ref readonly Argument<T> IsEqualTo(T? other)
        {
            if (!EqualityComparer<T>.Default.Equals(argument.Value!, other!))
                throw ArgRangeException.Create(argument, $"was not equal to {other}");
            return ref argument;
        }
    }

    extension<T>(in Argument<T> argument)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public ref readonly Argument<T> IsNotNull()
        {
            if (argument.Value is null)
                throw ArgNullException.Create(argument);
            return ref argument;
        }

        public ref readonly Argument<T> IsEqualTo(T? other, IEqualityComparer<T> comparer)
        {
            if (!comparer.Equals(argument.Value!, other!))
                throw ArgRangeException.Create(argument, $"was not equal to {Any.ToString(other)} according to {comparer}");
            return ref argument;
        }
    }
}