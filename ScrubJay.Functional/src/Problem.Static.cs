namespace ScrubJay.Functional;

partial class Problem
{
    /// <summary>
    /// Creates a new <see cref="Problem"/> about a <typeparamref name="T"/> <paramref name="argument"/>.
    /// </summary>
    /// <param name="argument">
    /// The <typeparamref name="T"/> argument that is invalid.
    /// </param>
    /// <param name="info">
    /// Optional additional information about why the argument is invalid.
    /// </param>
    /// <param name="argumentName">
    /// Automatically captured name of the argument parameter.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of the <paramref name="argument"/>.<br/>
    /// In <c>.NET 9.0+</c>, this may be a <c>ref struct</c> type.
    /// </typeparam>
    /// <returns>
    /// A new <see cref="Problem"/> with:<br/>
    /// - <see cref="Details"/> about the <paramref name="argument"/> and why it was invalid.<br/>
    /// - <see cref="Exception"/> is an <see cref="ArgumentException"/>.<br/>
    /// - <see cref="Data"/> contains more information about the <paramref name="argument"/>.
    /// </returns>
    public static Problem Argument<T>(T? argument, string? info = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var builder = StringBuilder.Rent()
            .Append($"Argument \"{argumentName ?? nameof(argument)}\": {TypeName.For<T>()} = `{Any.ToString(argument)}` is invalid");

        if (info is not null)
        {
            builder.Append(": ")
                .Append(info);
        }

        string details = builder.ToStringAndReturn();

        return new Problem
               {
                   Title = "Invalid Argument",
                   Details = details,
                   Exception = new ArgumentException(
                       details,
                       argumentName),
                   Data =
                   {
                       { "ArgumentName", argumentName },
                       { "ArgumentType", Any.GetType(argument) },
                       { "ArgumentValueString", Any.ToString(argument) },
                   },
               };
    }

    /// <summary>
    /// Creates a new <see cref="Problem"/> about a <typeparamref name="T"/> <paramref name="argument"/> being <c>null</c>.
    /// </summary>
    /// <param name="argument">
    /// The <typeparamref name="T"/> argument that is null.
    /// </param>
    /// <param name="argumentName">
    /// Automatically captured name of the argument parameter.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of the <paramref name="argument"/>.
    /// </typeparam>
    /// <returns>
    /// A new <see cref="Problem"/> with:<br/>
    /// - <see cref="Details"/> about the <c>null</c> <paramref name="argument"/>.<br/>
    /// - <see cref="Exception"/> is an <see cref="ArgumentNullException"/>.<br/>
    /// - <see cref="Data"/> contains more information about the <paramref name="argument"/>.
    /// </returns>
    public static Problem ArgumentNull<T>(T? argument,[CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        string details = StringBuilder.Rent()
            .Append($"Argument \"{argumentName ?? nameof(argument)}\": {TypeName.For<T>()} is null")
            .ToStringAndReturn();

        return new Problem
               {
                   Title = "Invalid Argument",
                   Details = details,
                   Exception = new ArgumentNullException(argumentName, details),
                   Data =
                   {
                       { "ArgumentName", argumentName },
                       { "ArgumentType", Any.GetType(argument) },
                   },
               };
    }
}