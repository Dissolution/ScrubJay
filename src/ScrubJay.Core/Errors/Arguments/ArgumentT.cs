namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public readonly ref struct Argument<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static implicit operator Argument(Argument<T> argument)
    {
        if (!Anything.TryBox(argument.Value, out var value))
        {
            value = Anything.ToString(argument.Value);
        }

        return new Argument(argument.Name, argument.Type, value);
    }

    public static Argument<T> Capture(
        T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument<T>(
            argumentName,
            Anything.GetType<T>(in argument),
            argument);
    }

    public readonly string? Name;
    public readonly Type Type;
    public readonly T? Value;

    public Argument(string? name, Type type, T? value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    public void Deconstruct(out string? name, out Type type, out T? value)
    {
        name = Name;
        type = Type;
        value = Value;
    }

    public override string ToString()
    {
        return $"\"{Name}\": {Type.Alias} = {Anything.ToString(in Value)}";
    }
}