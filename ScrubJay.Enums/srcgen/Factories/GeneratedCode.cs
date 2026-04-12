using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SGF;

namespace ScrubJay.Enums.SourceGen.Factories;

public readonly record struct GeneratedCode
{
    public readonly string HintName;
    public readonly string Code;

    public GeneratedCode(string hintName, string code)
    {
        HintName = hintName;
        Code = code;
    }
}

public static class GeneratedCodeExtensions
{
    public static void AddGeneratedCode(
        this IncrementalGeneratorPostInitializationContext context, 
        GeneratedCode generatedCode)
    {
        context.AddSource(generatedCode.HintName, SourceText.From(generatedCode.Code, Encoding.UTF8));
    }
    
    public static void AddGeneratedCode(
        this SgfSourceProductionContext context, 
        GeneratedCode generatedCode)
    {
        context.AddSource(generatedCode.HintName, SourceText.From(generatedCode.Code, Encoding.UTF8));
    }
}