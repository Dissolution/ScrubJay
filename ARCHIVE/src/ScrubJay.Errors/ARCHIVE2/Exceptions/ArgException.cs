//using ScrubJay.Errors.Arguments;
//using ScrubJay.Errors.Utilities;
//
//namespace ScrubJay.Errors.Exceptions;
//
///// <summary>
///// An enhanced <see cref="ArgumentException"/>.
///// </summary>
//[PublicAPI]
//public class ArgException : ArgumentException, IArgumentException<ArgException>
//{
//#region Throw / Create
//    [DoesNotReturn]
//    public static void Throw<T>(
//        in T? argument,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        throw Create(in argument, info, innerException, argumentName);
//    }
//
//    [DoesNotReturn]
//    public static void Throw(
//        ArgumentInfo argumentInfo,
//        string? info = null,
//        Exception? innerException = null)
//    {
//        throw Create(argumentInfo, info, innerException);
//    }
//
//    public static ArgException Create<T>(
//        in T? argument,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = ArgumentInfo.Capture<T>(in argument, argumentName);
//        return Create(arg, info, innerException);
//    }
//
//    public static ArgException Create(
//        ArgumentInfo argumentInfo,
//        string? info = null,
//        Exception? innerException = null)
//    {
//        DefaultInterpolatedStringHandler builder = $"ArgException - {argumentInfo}";
//        if (string.IsNullOrEmpty(info))
//        {
//            builder.AppendLiteral(" was invalid");
//        }
//        else
//        {
//            builder.AppendLiteral(" ");
//            builder.AppendLiteral(info!);
//        }
//        var message = builder.ToStringAndClear();
//
//        return new ArgException(argumentInfo, message, innerException);
//    }
//#endregion
//
//
//    public ArgumentInfo ArgumentInfo { get; }
//
//    public override string Message => ExceptionAccess.RefMessageField(this) ?? string.Empty;
//
//    public ArgException(
//        ArgumentInfo argument,
//        string? message = null,
//        Exception? innerException = null)
//        : base(message, argument.Name, innerException)
//    {
//        ArgumentInfo = argument;
//    }
//}