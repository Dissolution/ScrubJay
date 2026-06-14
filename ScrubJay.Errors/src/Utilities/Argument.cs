using Any = ScrubJay.Universal.Any;

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
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new Argument(Any.GetType<T>(in argument), argumentName, Any.Render<T?>(in argument));
    }

    public static Argument Capture<T>(
        scoped ReadOnlySpan<T> argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new Argument(typeof(ReadOnlySpan<T>), argumentName, Any.Render<T>(argument));
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

    public static Argument Null() => new(null, null, null);

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

    public override string ToString() => TextBuilder.Build(RenderTo);

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
            builder.IfAppend(wrote, ' ').RenderType(Type);
            wrote = true;
        }

        if (!string.IsNullOrEmpty(ValueString))
        {
            builder.IfAppend(wrote, " = ").Append(ValueString);
            wrote = true;
        }

        if (!wrote)
        {
            builder.Append("null");
        }
    }
}