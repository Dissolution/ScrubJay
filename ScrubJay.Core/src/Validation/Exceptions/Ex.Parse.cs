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
            InputType = Any.GetType<I>(in input),
            InputString = Any.ToString<I>(in input),
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
    
    public static ParseException Parse(
        scoped text input,
        Type outputType,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? inputName = null)
    {
        return new ParseException(inputName)
        {
            InputType = typeof(text),
            InputString = input.ToString(),
            OutputType = outputType,
            Info = info,
        };
    }
    
    public static ParseException Parse(
        string? input,
        Type outputType,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? inputName = null)
    {
        return new ParseException(inputName)
        {
            InputType = typeof(string),
            InputString = input,
            OutputType = outputType,
            Info = info,
        };
    }
}