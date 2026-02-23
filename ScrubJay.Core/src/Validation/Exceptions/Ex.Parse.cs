using ScrubJay.Parsing;


namespace ScrubJay.Validation;

partial class Ex
{
    public static ParseException Parse<I, O>(I? input,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? inputName = null)
#if NET9_0_OR_GREATER
        where I : allows ref struct
        where O : allows ref struct
#endif
    {
        return new ParseException(inputName)
        {
            InputType = Any.GetType<I>(input),
            InputString = Any.ToString<I>(input),
            OutputType = typeof(O),
            Info = info,
        };
    }
    
    public static ParseException Parse<O>(
        scoped text input,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? inputName = null)
#if NET9_0_OR_GREATER
        where O : allows ref struct
#endif
    {
        return new ParseException(inputName)
        {
            InputType = typeof(text),
            InputString = input.ToString(),
            OutputType = typeof(O),
            Info = info,
        };
    }
    
    public static ParseException Parse<O>(
        string? input,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? inputName = null)
#if NET9_0_OR_GREATER
        where O : allows ref struct
#endif
    {
        return new ParseException(inputName)
        {
            InputType = typeof(string),
            InputString = input,
            OutputType = typeof(O),
            Info = info,
        };
    }
}