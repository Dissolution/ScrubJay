using ScrubJay.Universal;

namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public record class Argument
{
    public static Argument Capture<T>(
        ref readonly T? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new(argumentName, Any.GetType<T>(in value), Any.ToString(in value));

    public static Argument Capture(
        object? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        => new(argumentName, Any.GetType(value), value?.ToString());


    public static Argument Create(string? name, object? value)
        => new(name, Any.GetType(value), value?.ToString());

    public static Argument Create<T>(
        string? name,
        ref readonly T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new(name, Any.GetType(in value), Any.ToString(in value));


    public static Argument Create(
        string? name,
        Type? type,
        string? display)
        => new(name, type, display);



    public required string? Name { get; init; }

    public required Type? Type { get; init; }

    public required string? ValueString { get; init; }

    public bool ContainsNull => Type is null && ValueString is null;

    public Argument() { }

    [SetsRequiredMembers]
    public Argument(string? name, Type? type, string? valueString)
    {
        Name = name;
        Type = type;
        ValueString = valueString;
    }
}