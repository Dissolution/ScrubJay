using ScrubJay.Polyfills.Text;
using ScrubJay.Polyfills.Universal;

namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public readonly record struct ArgumentInfo
{
    public static ArgumentInfo Capture<T>(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new ArgumentInfo(
            type: Any.GetType(argument),
            name: argumentName,
            valueString: Any.ToString(argument) ?? "null"
        );
    }

    public readonly Type Type;
    public readonly string? Name;
    public readonly string ValueString;

    internal ArgumentInfo(Type type, string? name, string valueString)
    {
        Type = type;
        Name = name;
        ValueString = valueString;
    }

    public void Deconstruct(out Type? type, out string? name, out string? valueString)
    {
        type = Type;
        name = Name;
        valueString = ValueString;
    }

    public override string ToString()
    {
        using var text = new InterpolatedText();
        text.Write('"');
        text.Write(Name);
        text.Write("\": ");
        text.Write(Type);
        text.Write(" = ");
        text.Write(ValueString);
        return text.ToString();
    }
}