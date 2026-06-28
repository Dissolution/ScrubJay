using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;
using ScrubJay.Polyfills.Universal;

namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public readonly ref struct Argument<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{


#if NET9_0_OR_GREATER
    public static Argument<T> Capture(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument<T>(
            type: Any.GetType(in argument),
            name: argumentName,
            value: argument,
            valueString: Any.ToString(in argument) ?? "null"
        );
    }
#else
    public static Argument<T> Capture(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument<T>(
            type: argument?.GetType() ?? typeof(T),
            name: argumentName,
            value: argument,
            valueString: argument?.ToString() ?? "null"
        );
    }

#endif


    public readonly Type Type;
    public readonly string? Name;
    public readonly T? Value;
    public readonly string ValueString;

    private Argument(Type type, string? name, T? value, string valueString)
    {
        Type = type;
        Name = name;
        Value = value;
        ValueString = valueString;
    }

    public void Deconstruct(out Type? type, out string? name, out T? value, out string? valueString)
    {
        type = Type;
        name = Name;
        value = Value;
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