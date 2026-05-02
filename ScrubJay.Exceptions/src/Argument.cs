using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

/// <summary>
/// Information about an argument captured for an <see cref="ArgumentException"/>.
/// </summary>
public record class Argument : IRenderable
{
    public static Argument Capture<T>(in T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        return new Argument(Any.GetType<T>(in argument), argumentName, Any.ToString<T>(in argument));
    }

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public static Argument Capture<T>(in T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return new Argument(Any.GetType<T>(in argument, _), argumentName, Any.ToString<T>(in argument, _));
    }
#endif

    public static Argument Capture(object? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        return new Argument(Any.GetType(argument), argumentName, Any.ToString(argument));
    }

    public static readonly Argument Null = new(null, null, null);


    /// <summary>
    /// The <see cref="Type"/> of the argument.
    /// </summary>
    public required Type? Type { get; init; }

    /// <summary>
    /// The name of the argument.
    /// </summary>
    public required string? Name { get; init; }

    /// <summary>
    /// A <see cref="string"/> representation of the Argument's value.
    /// </summary>
    public required string? ValueString { get; init; }

    public Argument()
    {

    }

    [SetsRequiredMembers]
    public Argument(Type? type, string? name, string? valueString)
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

    public override string ToString() => $"{Type?.FullName} {Name} = {ValueString}";

    public void RenderTo(TextBuilder builder)
    {
        builder.Append($"'{Name}'");

        if (Type is not null || ValueString is not null)
        {
            if (Type is not null)
            {
                builder.Append($" ({Type:@})");
            }

            if (ValueString is not null)
            {
                builder.Append(" = ").Append(ValueString);
            }
        }
        else
        {
            builder.Append(" (null)");
        }
    }
}