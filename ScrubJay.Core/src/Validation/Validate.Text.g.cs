#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<string> IsNotNull([AllowNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            return Ex.ArgNull(strName);
        return str;
    }
    
    public static Result<string> IsNotEmpty([AllowNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            return Ex.ArgNull(strName);
        if (str.Length == 0)
            return Ex.Arg(str, "was empty", strName);
        return str;
    }
    
    public static Result<string> IsNotWhitespace([AllowNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            return Ex.ArgNull(strName);
        if (str.Length == 0)
            return Ex.Arg(str, "was empty", strName);
        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsWhiteSpace(str[i]))
                return str;
        }

        return Ex.Arg(str, "was whitespace", strName);
    }
    
#if NET9_0_OR_GREATER
    
    public static RefResult<ReadOnlySpan<char>> IsNotEmpty(ReadOnlySpan<char> text,
        [CallerArgumentExpression(nameof(text))]
        string? textName = null)
    {
        if (text.IsEmpty)
            return Ex.Arg(text, "was empty", textName);
        return text;
    }
    
    public static RefResult<ReadOnlySpan<char>> IsNotWhitespace(ReadOnlySpan<char> text,
        [CallerArgumentExpression(nameof(text))]
        string? textName = null)
    {
        if (text.IsEmpty)
            return Ex.Arg(text, "was empty", textName);
        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
                return text;
        }

        return Ex.Arg(text, "was whitespace", textName);
    }

#endif
    
    public static Result<char> IsNotWhitespace(char ch,
        [CallerArgumentExpression(nameof(ch))]
        string? charName = null)
    {
        if (!char.IsWhiteSpace(ch))
            return ch;
        return Ex.Arg(ch, "was whitespace", charName);
    }
}
