namespace ScrubJay.Enums.SourceGen.Coding;

partial class CodeBuilder
{
    public CodeBuilder Format<T>(T? value)
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

        Write(str);
        return this;
    }
    
    public CodeBuilder Format<T>(T? value, string? format, IFormatProvider? formatProvider = default)
    {
        string? str;
        
        if (value is IFormattable)
        {
            str = ((IFormattable)value).ToString(format, formatProvider);
        }
        else
        {
            str = value?.ToString();
        }

        Write(str);
        return this;
    }
}