//using System.ComponentModel;
//using ScrubJay.Errors.Arguments;
//using ScrubJay.Errors.Utilities;
//
//namespace ScrubJay.Errors.Exceptions;
//
//[PublicAPI]
//public sealed class InvalidEnumException :
//    InvalidEnumArgumentException,
//    IArgumentException<InvalidEnumException>
//{
//    [DoesNotReturn]
//    [StackTraceHidden]
//    public static void Throw<E>(
//        in E @enum,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(@enum))]
//        string? enumName = null)
//        where E : struct, Enum
//    {
//        throw Create<E>(in @enum, info, innerException, enumName);
//    }
//
//    [StackTraceHidden]
//    public static InvalidEnumException Create<E>(
//        in E @enum,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(@enum))]
//        string? enumName = null)
//        where E : struct, Enum
//    {
//        ArgumentInfo argumentInfo = ArgumentInfo.Capture(in @enum, enumName);
//        int invalidValue = ((IConvertible)@enum).ToInt32(null);
//        
//        DefaultInterpolatedStringHandler builder = $"InvalidEnumException - {invalidValue} is not a valid {typeof(E)} Enum value";
//        if (!string.IsNullOrEmpty(info))
//        {
//            builder.AppendLiteral(" - ");
//            builder.AppendLiteral(info!);
//        }
//        var message = builder.ToStringAndClear();
//
//        return new InvalidEnumException(argumentInfo, invalidValue, message, innerException);
//    }
//
//
//    public ArgumentInfo ArgumentInfo { get; }
//
//    public override string Message => ExceptionAccess.RefMessageField(this) ?? string.Empty;
//    
//    private InvalidEnumException(
//        ArgumentInfo argument,
//        int enumValue,
//        string? message = null,
//        Exception? innerException = null)
//        : base(argumentName: argument.Name, invalidValue: enumValue, enumClass: argument.Type)
//    {
//        ArgumentInfo = argument;
//        ExceptionAccess.RefMessageField(this) = message;
//        ExceptionAccess.RefInnerExceptionField(this) = innerException;
//    }
//}