using System.Runtime.CompilerServices;

namespace ScrubJay.Enums.SourceGen.Coding;

partial class CodeBuilder
{
    public CodeBuilder Append(char ch)
    {
        Write(ch);
        return this;
    }
    
    public CodeBuilder Append(scoped ReadOnlySpan<char> text)
    {
        Write(text);
        return this;
    }
    
    public CodeBuilder Append([InterpolatedStringHandlerArgument("")] ref InterpolatedCode code)
    {
        // writing has happened
        return this;
    }
}