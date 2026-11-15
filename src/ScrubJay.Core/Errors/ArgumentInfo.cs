using ScrubJay.Generics;
using ScrubJay.Generics.Extensions;
using ScrubJay.Text;

namespace ScrubJay.Errors;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly record struct ArgumentInfo
{
    public readonly string? Name;
    public readonly Type? Type;
    public readonly string? ValueString;

    public ArgumentInfo(string? name, Type? type, string? valueString)
    {
        Name = name;
        Type = type;
        ValueString = valueString;
    }

    public override string ToString()
    {
        #pragma warning disable CA2000
        return InterpolatedTextWriter
            .Create()
            .IfNull(Name,
                static i => i.Write("?name"),
                static (i, n) => i.Append('"').Append(n).Append('"'))
            .IfNotNull(Type,
                static (i, t) => i.Append(": ").Append(t.Alias))
            .Append(" = ")
            .Append(ValueString)
            .ToStringAndDispose();
    }
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly record struct Argument
{
    public static implicit operator ArgumentInfo(Argument argument) => new(argument.Name, argument.Type, argument.Value?.ToString());

    public static Argument Capture<T>(T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        return new Argument(argumentName, Anything.GetType(argument), (object?)argument);
    }

    public static ArgumentInfo Info<T>(T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        return new ArgumentInfo(argumentName, typeof(T), argument?.ToString());
    }

    public static ArgumentInfo Info<T>(
        T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return new ArgumentInfo(argumentName, Anything.GetType(argument), Anything.ToString(argument));
    }

    public readonly string? Name;
    public readonly Type? Type;
    public readonly object? Value;

    public Argument(string? name, Type? type, object? value)
    {
        Name = name;
        Type = type;
        Value = value;
    }
}