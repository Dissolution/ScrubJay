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
}
