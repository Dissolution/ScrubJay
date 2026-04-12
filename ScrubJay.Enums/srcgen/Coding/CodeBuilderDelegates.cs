global using static ScrubJay.Enums.SourceGen.Coding.CodeBuilderDelegates;

namespace ScrubJay.Enums.SourceGen.Coding;

public static class CodeBuilderDelegates
{
    public delegate void CodeBuilderAction(CodeBuilder builder);
    
    public delegate void CodeBuilderValueAction<in T>(CodeBuilder builder, T value);
}