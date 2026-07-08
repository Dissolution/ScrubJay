namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public readonly ref struct Argument<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static implicit operator ArgumentInfo(Argument<T> argument)
    {
        return new ArgumentInfo(argument.Type, argument.Name, Any.ToString<T>(in argument.Value) ?? "null");
    }
    
    public static Argument<T> Capture(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument<T>(
            Any.GetType<T>(in argument),
            argumentName,
            argument);
    }

    public readonly Type Type;
    public readonly string? Name;
    public readonly T? Value;

    internal Argument(Type type, string? name, in T? value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"\"{Name}\": {Type} = {Any.ToString(in Value)}";
    }
}