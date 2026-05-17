#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;

namespace ScrubJay.Reflection.Utilities;

/// <summary>
/// 
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names"/>
[PublicAPI]
public static class Naming
{
#if NET8_0_OR_GREATER
    private static readonly FrozenSet<string> _keywords = SyntaxFacts
        .GetKeywordKinds()
        .Select(SyntaxFacts.GetText)
        .ToFrozenSet();
#else
    private static readonly HashSet<string> _keywords = SyntaxFacts
        .GetKeywordKinds()
        .Select(SyntaxFacts.GetText)
        .ToHashSet(StringComparer.Ordinal);

#endif


    private static bool IsValidFirstCharacter(char ch)
    {
        if (ch == '_')
            return true;

        var category = char.GetUnicodeCategory(ch);
        return category is UnicodeCategory.UppercaseLetter
            or UnicodeCategory.LowercaseLetter
            or UnicodeCategory.TitlecaseLetter
            or UnicodeCategory.ModifierLetter
            or UnicodeCategory.LetterNumber;
    }

    private static bool IsValidCharacter(char ch)
    {
        var category = char.GetUnicodeCategory(ch);
        return category is UnicodeCategory.UppercaseLetter
            or UnicodeCategory.LowercaseLetter
            or UnicodeCategory.TitlecaseLetter
            or UnicodeCategory.ModifierLetter
            or UnicodeCategory.LetterNumber
            or UnicodeCategory.DecimalDigitNumber
            or UnicodeCategory.ConnectorPunctuation
            or UnicodeCategory.NonSpacingMark
            or UnicodeCategory.SpacingCombiningMark
            or UnicodeCategory.Format;
    }

    public static bool IsValidIdentifier(scoped text identifier)
    {
        if (identifier.Length == 0)
            return false;

        char ch = identifier[0];
        if (!IsValidFirstCharacter(ch))
            return false;
        for (var i = 1; i < identifier.Length; i++)
        {
            if (!IsValidCharacter(identifier[i]))
                return false;
        }

        return true;
    }

    public static string FixIdentifier(scoped text identifier)
    {
        Guard.IsNotEmpty(identifier);
        using var builder = new TextBuilder();

        int i = 0;
        char ch = identifier[0];
        if (IsValidFirstCharacter(ch))
        {
            builder.Append(ch);
            i++;
        }
        else
        {
            builder.Append('_');
        }

        for (; i < identifier.Length; i++)
        {
            ch = identifier[i];
            if (IsValidCharacter(ch))
            {
                builder.Append(ch);
            }
            else
            {
                builder.Append('_');
            }
        }

        string name = builder.ToString();
        if (_keywords.Contains(name))
        {
            builder.Insert(0, '@');
            return builder.ToString();
        }

        return name;
    }

#if NETSTANDARD2_0 || NETFRAMEWORK
    public static string FixIdentifier(string? identifier) => FixIdentifier(identifier.AsSpan());
#endif
}