namespace ScrubJay.Validation;

partial class Guard
{
    public static string IsNotNull([AllowNull, NotNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            throw Ex.ArgNull(strName);
        return str;
    }
    
    public static string IsNotEmpty([AllowNull, NotNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            throw Ex.ArgNull(strName);
        if (str.Length == 0)
            throw Ex.Arg(str, "was empty", strName);
        return str;
    }
    
    public static string IsNotWhitespace([AllowNull, NotNull] string? str,
        [CallerArgumentExpression(nameof(str))]
        string? strName = null)
    {
        if (str is null)
            throw Ex.ArgNull(strName);
        if (str.Length == 0)
            throw Ex.Arg(str, "was empty", strName);
        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsWhiteSpace(str[i]))
                return str;
        }

        throw Ex.Arg(str, "was whitespace", strName);
    }
    
//#if NET9_0_OR_GREATER
    
    public static ReadOnlySpan<char> IsNotEmpty(ReadOnlySpan<char> text,
        [CallerArgumentExpression(nameof(text))]
        string? textName = null)
    {
        if (text.IsEmpty)
            throw Ex.Arg(text, "was empty", textName);
        return text;
    }
    
    public static ReadOnlySpan<char> IsNotWhitespace(ReadOnlySpan<char> text,
        [CallerArgumentExpression(nameof(text))]
        string? textName = null)
    {
        if (text.IsEmpty)
            throw Ex.Arg(text, "was empty", textName);
        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
                return text;
        }

        throw Ex.Arg(text, "was whitespace", textName);
    }

//#endif
    
    public static char IsNotWhitespace(char ch,
        [CallerArgumentExpression(nameof(ch))]
        string? charName = null)
    {
        if (!char.IsWhiteSpace(ch))
            return ch;
        throw Ex.Arg(ch, "was whitespace", charName);
    }
}