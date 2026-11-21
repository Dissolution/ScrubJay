namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public sealed record class Argument
{
    public static Argument<T> Capture<T>(
        T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Argument<T>.Capture(argument, argumentName);

    [OverloadResolutionPriority(100)]
    public static Argument<T> Null<T>(
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Argument<T>.Capture(default, argumentName);

    [OverloadResolutionPriority(80)]
    public static Argument<T> Null<T>(
        T? argument = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Argument<T>.Capture(argument, argumentName);


    public string? Name { get; init; }
    public Type? Type { get; init; }
    public object? Value { get; init; }

    public Argument()
    {
    }

    public Argument(string? name, Type? type, object? value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public void Deconstruct(out string? name, out Type? type, out object? value)
    {
        name = Name;
        type = Type;
        value = Value;
    }

    public override string ToString()
    {
        return $"\"{Name}\": {Type.Alias} = {Value}";
    }
}