using ScrubJay.Functional;
using ScrubJay.Universal;

namespace ScrubJay.Validation;

public interface IArgument
{
    string? Name { get; }
    Type? Type { get; }
    string? ValueString { get; }
}

public interface IArgument<out T> : IArgument
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    T Value { get; }

#if !NETSTANDARD2_0 && !NETFRAMEWORK
    Type IArgument.Type => Any.GetType<T>(Value);
    string? IArgument.ValueString => Any.ToString(Value);
#endif
}

public readonly record struct Arg(string? Name, Type? Type, string? ValueString) : IArgument
{
    public static Arg New<A>(A argument)
        where A : struct, IArgument
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
    {
        return new Arg(argument.Name, argument.Type, argument.ValueString);
    }
}

public readonly record struct Arg<T>(string? Name, Type? Type, T Value) : IArgument<T>, IArgument
{
    public static Arg<T> New<A>(A argument)
        where A : struct, IArgument<T>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
    {
        return new Arg<T>(argument.Name, argument.Type, argument.Value);
    }

    public string ValueString => Any.ToString(Value);
}

public readonly
#if NET9_0_OR_GREATER
    ref
#endif
    struct Argument : IArgument
{
    public static Argument<T> New<T>(
        T value,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new Argument<T>(valueName, Any.GetType(value), value);
    }

    public readonly string? Name;
    public readonly Type? Type;
    public readonly string? ValueString;

    string? IArgument.Name => Name;

    Type? IArgument.Type => Type;

    string? IArgument.ValueString => ValueString;

    internal Argument(string? name, Type? type, string? valueString)
    {
        Name = name;
        Type = type;
        ValueString = valueString;
    }

    public void Deconstruct(out string? name, out Type? type, out string? valueString)
    {
        name = Name;
        type = Type;
        valueString = ValueString;
    }
}

public readonly
#if NET9_0_OR_GREATER
    ref
#endif
    struct Argument<T> : IArgument<T>, IArgument
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public readonly string? Name;
    public readonly Type? Type;
    public readonly T Value;

    string? IArgument.Name => Name;

    Type? IArgument.Type => Type;

    T IArgument<T>.Value => Value;

    string IArgument.ValueString => Any.ToString(Value);

    internal Argument(string? name, Type? type, T value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public void Deconstruct(out string? name, out Type? type, out T value)
    {
        name = Name;
        type = Type;
        value = Value;
    }
}