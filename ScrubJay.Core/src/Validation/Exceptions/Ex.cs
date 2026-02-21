namespace ScrubJay.Validation;

/// <summary>
/// A static helper class for creating <see cref="Exception">Exceptions</see>
/// using <see cref="InterpolatedTextBuilder"/> to generate messages
/// </summary>
[PublicAPI]
[StackTraceHidden]
public static partial class Ex
{
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