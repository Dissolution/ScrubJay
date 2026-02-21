namespace ScrubJay.Validation;

partial class Ex
{
    public static NotSupportedException ThisMethodIsNotSupported(
        Type? instanceType = null,
        string? info = null,
        [CallerMemberName] string? methodName = null)
    {
        string message = TextBuilder
            .New
            .Append($"Cannot call {instanceType:@}.{methodName}")
            .IfNotEmpty(info,
                static (builder, n) => builder.Append(": ").Write(n),
                builder => builder.If(instanceType!.IsRef,
                    tb => tb.Append($": {instanceType:@} is a ref struct and cannot be boxed")))
            .ToStringAndDispose();

        return new NotSupportedException(message);
    }

    public static NotSupportedException ThisMethodIsNotSupported<T>(
        T? instance = default,
        string? info = null,
        [CallerMemberName] string? methodName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => ThisMethodIsNotSupported(typeof(T), info, innerException, methodName);


    public static NotSupportedException IsReadOnly<T>(
        T? instance,
        InterpolatedTextBuilder info = default,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(instance))]
        string? instanceName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = TextBuilder.New
            .Append($"Instance \"{instanceName}\" ({typeof(T):@}) `{instance}` is read-only")
            .AppendOptionalInfo(ref info)
            .ToStringAndDispose();
        throw new NotSupportedException(message);
    }

}