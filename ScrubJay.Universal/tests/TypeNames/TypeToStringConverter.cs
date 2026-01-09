using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SymbolDisplayFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat;

namespace ScrubJay.Universal.Tests.TypeNames;

public static class TypeToStringConverter
{
    private static readonly CSharpCompilation _compilation;

    private static readonly SymbolDisplayFormat _cleanFormat = new SymbolDisplayFormat(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);


    static TypeToStringConverter()
    {
        // Create a minimal compilation with core references
        var references = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(static assembly => File.Exists(assembly.Location))
            .Select(static assembly => MetadataReference.CreateFromFile(assembly.Location))
            .ToList();

        _compilation = CSharpCompilation.Create("ScrubJay", references: references);
    }

    public static string ToCSharpString(Type? type)
    {
        if (type is null)
            return "null";
        
        string? fullName = type.FullName;
        if (fullName is not null)
        {
            INamedTypeSymbol? symbol = _compilation.GetTypeByMetadataName(fullName);
            if (symbol is not null)
            {
                return symbol.ToDisplayString(_cleanFormat);
            }

            return fullName;
        }
        
        Debugger.Break();
        return "null";
    }
}