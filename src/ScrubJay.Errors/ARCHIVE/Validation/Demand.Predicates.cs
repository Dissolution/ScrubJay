//using System.Linq.Expressions;
//
//namespace ScrubJay.Errors.Validation;
//
//public partial class Demand
//{
//    [DoesNotReturn]
//    private static void ThrowPredicateFail<P>(P? predicate, bool expected, string? info, string? predicateName)
//    {
//        throw Ex.PredicateFail<P>(predicate, expected, info, predicateName);
//    }
//
//    public static void True(
//        [DoesNotReturnIf(false)] bool predicate,
//        string? info = null,
//        [CallerArgumentExpression(nameof(predicate))]
//        string? predicateName = null)
//    {
//        if (!predicate)
//            ThrowPredicateFail(predicate, true, info, predicateName);
//    }
//
//    public static void True(
//        [DoesNotReturnIf(false)] bool? predicate,
//        string? info = null,
//        [CallerArgumentExpression(nameof(predicate))]
//        string? predicateName = null)
//    {
//        if (predicate != true)
//            ThrowPredicateFail(predicate, true, info, predicateName);
//    }
//
//    public static void True(
//        [AllowNull, NotNull] Func<bool>? predicate,
//        string? info = null,
//        [CallerArgumentExpression(nameof(predicate))]
//        string? predicateName = null)
//    {
//        if (predicate is null)
//            Throw.ArgNull(predicate, info, predicateName);
//        if (!predicate.Invoke())
//            ThrowPredicateFail(predicate, true, info, predicateName);
//    }
//
//    public static void True(
//        [AllowNull, NotNull] Expression<Func<bool>>? predicate,
//        string? info = null,
//        [CallerArgumentExpression(nameof(predicate))]
//        string? predicateName = null)
//    {
//        if (predicate is null)
//            Throw.ArgNull(predicate, info, predicateName);
//        if (!predicate.Compile().Invoke())
//            ThrowPredicateFail(predicate, true, info, predicateName);
//    }
//}