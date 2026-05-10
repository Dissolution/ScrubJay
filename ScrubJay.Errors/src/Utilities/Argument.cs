using UNPROCESSED_Any = ScrubJay.Universal.UNPROCESSED.Any;

namespace ScrubJay.Errors.Utilities;

/// <summary>
/// Information about an argument used with Exceptions.
/// </summary>
[PublicAPI]
public sealed record class Argument : IRenderable
{
    public static Argument Capture<T>(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument(UNPROCESSED_Any.GetType<T>(in argument), argumentName, UNPROCESSED_Any.Render<T>(in argument));
    }

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public static Argument Capture<T>(
        in T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return new Argument(UNPROCESSED_Any.GetType<T>(in argument, _), argumentName, UNPROCESSED_Any.Render<T>(in argument, _));
    }
#endif

    public static Argument Capture<T>(
        scoped ReadOnlySpan<T> argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument(typeof(ReadOnlySpan<T>), argumentName, UNPROCESSED_Any.Render<T>(argument));
    }

    public static Argument Null<T>(string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new Argument(typeof(T), argumentName, null);
    }

    public static Argument Null<T>(
        T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Debug.Assert(argument is null);
        return new Argument(typeof(T), argumentName, null);
    }

#if NET9_0_OR_GREATER
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public static Argument Null<T>(
        T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        Debug.Assert(argument is null);
        return new Argument(typeof(T), argumentName, null);
    }
#endif

    /// <summary>
    /// The <see cref="Type"/> of the argument.
    /// </summary>
    public required Type? Type { get; init; }

    /// <summary>
    /// The name of the argument.
    /// </summary>
    public required string? Name { get; init; }

    /// <summary>
    /// A <see cref="string"/> rendering of the Argument's value.
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

    public override string ToString() => R($"'{Name}' ({Type:@}) = {ValueString}");

    public void RenderTo(TextBuilder builder)
    {
        bool wrote = false;

        if (!string.IsNullOrEmpty(Name))
        {
            builder.Append('\'').Append(Name).Append('\'');
            wrote = true;
        }

        if (Type is not null)
        {
            builder.If(wrote, TB.Write(' '))
                .RenderType(Type);
            wrote = true;
        }

        if (!string.IsNullOrEmpty(ValueString))
        {
            builder.If(wrote, TB.Write(" = "))
                .Append(ValueString);
            wrote = true;
        }

        if (!wrote)
        {
            builder.Append("null");
        }
    }
}