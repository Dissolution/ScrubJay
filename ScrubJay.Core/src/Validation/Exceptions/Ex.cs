using ScrubJay.Destructuring;

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

    internal static TextBuilder AppendArgument<T>(
        this TextBuilder builder,
        T value, string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return builder
            .Append('"')
            .If(valueName is not null, valueName, "<???>")
            .Append("\": ")
            .IfNotNull(value,
                static (tb, v) => tb.DestructType<T>().Append(" = `").Append(v).Append('`'),
                static tb => tb.Write("<null>"));
    }


#region InvalidOperationException

    /// <summary>
    /// Get a new <see cref="InvalidOperationException"/>
    /// </summary>
    public static InvalidOperationException Invalid(
        InterpolatedTextBuilder message = default,
        Exception? innerException = null)
    {
        return new InvalidOperationException(
            message.ToStringAndClear(),
            innerException);
    }

#endregion /InvalidOperationException

#region NotImplementedException

    /// <summary>
    /// Get a new <see cref="NotImplementedException"/>
    /// </summary>
    public static NotImplementedException NotImplemented(
        InterpolatedTextBuilder message = default,
        Exception? innerException = null)
    {
        return new NotImplementedException(
            message.ToStringAndClear(),
            innerException);
    }

#endregion /NotImplementedException

    #region UnreachableException
    /// <summary>
    /// Get a new <see cref="UnreachableException"/>
    /// </summary>
    public static UnreachableException Unreachable(
        InterpolatedTextBuilder message = default,
        Exception? innerException = null)
    {
        return new UnreachableException(
            message.ToStringAndClear(),
            innerException);
    }
    #endregion /UnreachableException


}