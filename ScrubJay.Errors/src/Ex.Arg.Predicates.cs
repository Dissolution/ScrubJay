namespace ScrubJay.Errors;

partial class Ex
{
    internal static ArgException PredicateFail<P>(
        P? predicate, 
        bool expected, 
        string? info = null,
        [CallerArgumentExpression(nameof(predicate))]
        string? predicateName = null)
    {
        var arg = Argument.Capture<P>(predicate, predicateName);
        var message = TextBuilder.Rent()
            .Append($"Predicate {arg:@} was not {expected:@}")
            .AppendInfo(info)
            .ToStringAndDispose();
        throw new ArgException(arg, message);
    }
}