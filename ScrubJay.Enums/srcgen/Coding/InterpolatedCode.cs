using System.Runtime.CompilerServices;


namespace ScrubJay.Enums.SourceGen.Coding;

[InterpolatedStringHandler]
public ref struct InterpolatedCode
{
    private readonly CodeBuilder _builder;

    public InterpolatedCode(int literalLength, int formattedCount, CodeBuilder builder)
    {
        _builder = builder;
        _builder.Adding(literalLength + (formattedCount * 16));
    }

    public void AppendLiteral(string literal)
    {
        _builder.Write(literal);
    }

    public void AppendFormatted(char ch)
    {
        _builder.Write(ch);
    }
    
    public void AppendFormatted(scoped ReadOnlySpan<char> text)
    {
        _builder.Write(text);
    }

    public void AppendFormatted<T>(T? value)
    {
        string? str;
        if (value is IFormattable)
        {
            str = ((IFormattable)value).ToString(default, default);
            
        }
        else
        {
            str = value?.ToString();
        }
        _builder.Write(str);
    }
    
    
    public void AppendFormatted<T>((CodeBuilderValueAction<T> Action, T Value) tuple)
    {
        _builder.InterpolatedDelegate<T>(tuple.Action, tuple.Value);
    }
}