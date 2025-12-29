namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Demand
{
    public static Argument<T> That<T>(T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return Argument.New<T>(value, valueName);
    }
    //
    // public static ExpressionActual<T> That<T>(
    //     Expression<Func<T>> expression,
    //     [CallerArgumentExpression(nameof(expression))]
    //     string? expressionName = null)
    // {
    //     var actual = new ExpressionActual<T>()
    //     {
    //         Expression = expression,
    //         ValueName = expressionName,
    //         Value = expression.Compile().Invoke(),
    //     };
    //     return actual;
    // }
}
//
//
// public class ExpressionActual<T> : IActual<T>
// {
//     public Expression Expression { get; init; }
//
//     public string? ValueName { get; init; }
//
//     public Type? ValueType => Any.GetType(Value);
//     
//     public T Value { get; init; }
//     
//     public string? ValueString => Value?.ToString();
//
//     public IActual Realize() => this;
// }