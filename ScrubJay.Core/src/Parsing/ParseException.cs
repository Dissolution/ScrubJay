#pragma warning disable CA1032, CA1710, CA1010

namespace ScrubJay.Parsing;

/// <summary>
/// A <see cref="ParseException"/> is a type of <see cref="ArgumentException"/> that contains additional information
/// about the input that failed to parse and the <see cref="Type"/> it failed to parse into.
/// </summary>
/// <remarks>
/// <see cref="ParseException"/> supports fluent instantiation of its <see cref="Exception.Data"/>.
/// </remarks>
[PublicAPI]
public sealed class ParseException : ArgumentException, IEnumerable
{
    /// <summary>
    /// The <see cref="Type"/> of input that failed to parse.
    /// </summary>
    public required Type InputType { get; init; }

    /// <summary>
    /// A <see cref="string"/> representation of the input value that failed to parse.
    /// </summary>
    public required string? InputString { get; init; }

    /// <summary>
    /// The <see cref="Type"/> that the input failed to parse into.
    /// </summary>
    public required Type OutputType { get; init; }

    /// <summary>
    /// Optional additional information about why parsing failed.
    /// </summary>
    public string? Info { get; init; }

    public override string Message
    {
        get
        {
            return TextBuilder.New
                .Append("Unable to parse ")
                .AppendArgument(ParamName, InputType, InputString)
                .Append(" into a ")
                .Append(Type.Render(OutputType))
                .Append(" value")
                .AppendOptionalInfo(Info)
                .ToStringAndDispose();
        }
    }

    internal ParseException(string? paramName, Exception? innerException = null)
        : base(null, paramName, innerException)
    {
        
    }

    /// <summary>
    /// Adds a key/value pair into <see cref="Exception.Data"/>
    /// </summary>
    /// <remarks>
    /// Support for fluent setting of Data during Exception creation
    /// </remarks>
    public void Add(string key, object? value)
    {
        Guard.IsNotEmpty(key);
        if (value is not null)
        {
            Data.Add(key, value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        // do nothing
        yield break;
    }
}