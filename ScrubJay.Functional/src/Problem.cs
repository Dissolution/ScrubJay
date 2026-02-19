#pragma warning disable CA1032, RCS1194

using System.Text;

namespace ScrubJay.Functional;

/// <summary>
/// A generic Problem for use as a non-<see cref="Exception"/> Error in a <see cref="Result{T,E}"/>.<br/>
/// This is a rough approximation of <see href="https://www.rfc-editor.org/rfc/rfc9457.html">Problem Details</see> without the overhead of any ASP or Web related properties.<br/>
/// </summary>
/// <remarks>
/// This class implements <see cref="IEnumerable"/> and has an <see cref="Add"/> method so that it can be used with fluent collection initialization syntax.
/// </remarks>
[PublicAPI]
public partial class Problem : IEnumerable<KeyValuePair<string, object?>>
{
    public static implicit operator Problem(Exception exception) => new Problem(exception);

    private Dictionary<string, object?>? _data;

    /// <summary>
    /// The details about this <see cref="Problem"/>
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// The title of this <see cref="Problem"/>
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// An optional <see cref="Exception"/> related to this <see cref="Problem"/>
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Additional contextual information about this <see cref="Problem"/>
    /// </summary>
    public IDictionary<string, object?> Data
        => _data ??= new Dictionary<string, object?>(
               capacity: 0,
               comparer: StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Directly gets or sets <see cref="Data"/>
    /// </summary>
    /// <param name="key"></param>
    public object? this[object? key] { get => Data[key?.ToString() ?? "null"]; set => Data[key?.ToString() ?? "null"] = value; }

    public Problem() { }

    public Problem(string? details)
        : this(
            details,
            null,
            null) { }

    public Problem(string? details, string? title)
        : this(
            details,
            title,
            null) { }

    public Problem(string? details, Exception? exception)
        : this(
            details,
            null,
            exception) { }

    public Problem(string? details, string? title, Exception? exception)
    {
        if (exception is not null)
        {
            this.Exception = exception;
            this.Details = details ?? exception.Message;
            this.Title = title ?? TypeName.For(exception);

            if (exception.Data.Count > 0)
            {
                _data = new(
                    capacity: exception.Data.Count,
                    StringComparer.OrdinalIgnoreCase);

                foreach (DictionaryEntry data in exception.Data)
                {
                    _data[data.Key.ToString() ?? "null"] = data.Value;
                }
            }
        }
        else
        {
            this.Exception = null;
            this.Details = details;
            this.Title = title;
        }
    }

    public Problem(Exception? exception)
        : this(
            null,
            null,
            exception) { }

    public Problem(Exception? exception, string? title)
        : this(
            null,
            title,
            exception) { }

    public void Add(string key, object? value)
    {
        this.Data[key] = value;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.Data.GetEnumerator();

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => this.Data.GetEnumerator();

    public override string ToString()
    {
        StringBuilder builder = StringBuilder.Rent();

        builder.AppendLine("Problem:")
            .Append("    Title: ")
            .Append(Title)
            .AppendLine()
            .Append("  Details: ")
            .Append(Details)
            .AppendLine();

        if (Exception is not null)
        {
            builder.Append("    Error: ")
                .Append(TypeName.For(Exception))
                .AppendLine()
                .Append("      ")
                .Append(Exception.Message)
                .AppendLine();
        }

        if (_data is not null && _data.Count > 0)
        {
            builder.Append("     Data:");

            foreach (var pair in _data)
            {
                builder.AppendLine()
                    .Append("       ")
                    .Append(pair.Key)
                    .Append(":  ")
                    .Append(pair.Value);
            }
        }

        return builder.ToStringAndReturn();
    }
}

#pragma warning disable CA1010, MA0056
[PublicAPI]
public class ProblemException : Exception
{
    public string? Title { get; }

    public ProblemException(Problem problem)
        : base(
            problem.Details,
            problem.Exception)
    {
        this.Title = problem.Title;

        foreach (var kvp in problem.Data)
        {
            base.Data.Add(
                kvp.Key,
                kvp.Value);
        }
    }

    public ProblemException(string? details, string? title = null, Exception? exception = null)
        : base(
            details,
            exception)
    {
        this.Title = title;
    }
}