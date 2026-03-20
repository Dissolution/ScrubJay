using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

[PublicAPI]
public sealed record class Argument
{
    public static Argument Create(string? name, Type? type, string? display)
        => new(name, type, display);
    
    public static Argument Create(string? name, object? value)
        => new(name, value?.GetType(), value?.ToString());

    public static Argument Create<T>(string? name, T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new(name, Any.GetType(in value), Any.ToString(in value));

    public static Argument Create(object? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        => new(argumentName, value?.GetType(), value?.ToString());

    public static Argument Create<T>(T? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => new(argumentName, Any.GetType(in value), Any.ToString(in value));

    public required string? Name { get; init; }
    
    public required Type? Type { get; init; }
    
    public required string? ValueString { get; init; }

    public bool ContainsNull => Type is null && ValueString is null;
    
    public Argument() { }

    [SetsRequiredMembers]
    public Argument(string? name, Type? type, string? valueString)
    {
        this.Name = name;
        this.Type = type;
        this.ValueString = valueString;
    }
}