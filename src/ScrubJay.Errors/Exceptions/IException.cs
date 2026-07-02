using ScrubJay.Errors.Arguments;
#pragma warning disable CA1711

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public interface IException<out S>
    where S : Exception, IException<S>
{
//#if NET7_0_OR_GREATER
//    [DoesNotReturn]
//    [StackTraceHidden]
//    static virtual void Throw(string? message = null, Exception? innerException = null)
//    {
//        throw S.Create(message, innerException);
//    }
//
//    [StackTraceHidden]
//    static abstract S Create(string? message = null, Exception? innerException = null);
//#endif
}

[PublicAPI]
public interface IArgumentException<out S> : IException<S>
    where S : Exception, IArgumentException<S>
{
//#if NET7_0_OR_GREATER
//    [DoesNotReturn]
//    [StackTraceHidden]
//    static virtual void Throw(ArgumentInfo argument, string? info = null, Exception? innerException = null)
//    {
//        throw S.Create(argument, info, innerException);
//    }
//
//    [DoesNotReturn]
//    [StackTraceHidden]
//    static virtual void Throw<T>(
//        in T? argument,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        throw S.Create<T>(argument, info, innerException, argumentName);
//    }
//
//    [StackTraceHidden]
//    static abstract S Create(ArgumentInfo argument, string? info = null, Exception? innerException = null);
//
//    [StackTraceHidden]
//    static abstract S Create<T>(
//        in T? argument,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    ;
//
//#endif

    ArgumentInfo ArgumentInfo { get; }
}