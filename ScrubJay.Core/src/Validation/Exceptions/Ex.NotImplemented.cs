namespace ScrubJay.Validation;

static partial class Ex
{
    /// <summary>
    /// Get a new <see cref="NotImplementedException"/>
    /// </summary>
    public static NotImplementedException NotImplemented() => new();

    /// <summary>
    /// Get a new <see cref="NotImplementedException"/> with a <see cref="string"/> <paramref name="message"/>.
    /// </summary>
    public static NotImplementedException NotImplemented(string? message) => new(message);

    /// <summary>
    /// Get a new <see cref="NotImplementedException"/> with an <see cref="InterpolatedTextBuilder"/> message.
    /// </summary>
    public static NotImplementedException NotImplemented(ref InterpolatedTextBuilder message)
    {
        return new NotImplementedException(message.ToStringAndDispose());
    }
}