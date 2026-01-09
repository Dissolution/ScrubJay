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
}