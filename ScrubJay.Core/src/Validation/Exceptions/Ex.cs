namespace ScrubJay.Validation;

/// <summary>
/// A static helper class for creating <see cref="Exception">Exceptions</see>
/// using <see cref="InterpolatedTextBuilder"/> to generate messages
/// </summary>
[PublicAPI]
[StackTraceHidden]
public static partial class Ex
{
    internal static TextBuilder AppendOptionalInfo(this TextBuilder builder, string? info)
    {
        if (!string.IsNullOrEmpty(info))
        {
            return builder.Append(": ").Append(info);
        }

        return builder;
    }

    internal static TextBuilder AppendOptionalInfo(
        this TextBuilder builder,
        [InterpolatedStringHandlerArgument(nameof(builder))]
        ref InterpolatedTextBuilder info)
    {
        if (info.Length > 0)
        {
            return builder.Append(": ").Append(ref info);
        }

        return builder;
    }

    [return: NotNullIfNotNull(nameof(argument))]
    internal static object? BoxOrToStringArg<T>(T? argument)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (Any.TryBox<T>(argument, out object? boxed))
            return boxed;
        return (object?)Any.ToString<T>(argument);
    }









#region UnreachableException
    /// <summary>
    /// Get a new <see cref="UnreachableException"/>
    /// </summary>
    public static UnreachableException Unreachable(
        InterpolatedTextBuilder message = default,
        Exception? innerException = null)
    {
        return new UnreachableException(
            message.ToStringAndDispose(),
            innerException);
    }
#endregion /UnreachableException


}