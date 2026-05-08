using System.Linq.Expressions;

namespace ScrubJay.Errors.Validation;

partial class Validate
{
    public static Result True(
        bool predicate,
        string? info = null,
        [CallerArgumentExpression(nameof(predicate))]
        string? predicateName = null)
    {
        if (!predicate)
            return Ex.PredicateFail(predicate, true, info, predicateName);
        return Result.Ok;
    }
    
    public static Result True(
        bool? predicate,
        string? info = null,
        [CallerArgumentExpression(nameof(predicate))]
        string? predicateName = null)
    {
        if (predicate != true)
            return Ex.PredicateFail(predicate, true, info, predicateName);
        return Result.Ok;
    }
    
    public static Result True(
        Func<bool>? predicate,
        string? info = null,
        [CallerArgumentExpression(nameof(predicate))]
        string? predicateName = null)
    {
        if (predicate is null)
            return Ex.ArgNull(predicate, info, predicateName);
        if (!predicate.Invoke())
            return Ex.PredicateFail(predicate, true, info, predicateName);
        return Result.Ok;
    }
    
    public static Result True(
        Expression<Func<bool>>? predicate,
        string? info = null,
        [CallerArgumentExpression(nameof(predicate))]
        string? predicateName = null)
    {
        if (predicate is null)
            return Ex.ArgNull(predicate, info, predicateName);
        if (!predicate.Compile().Invoke())
            return Ex.PredicateFail(predicate, true, info, predicateName);
        return Result.Ok;
    }
}