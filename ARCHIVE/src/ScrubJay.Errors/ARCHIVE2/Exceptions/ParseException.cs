//using ScrubJay.Errors.Collections;
//using ScrubJay.Errors.Utilities;
//
//namespace ScrubJay.Errors.Exceptions;
//
//[PublicAPI]
//public sealed class ParseException : FormatException, IException<ParseException>
//{
//#region Throw / Create
//    [DoesNotReturn]
//    [StackTraceHidden]
//    public static void Throw<T>(
//        string? input,
//        string? info = null,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        throw Create<T>(input, info, innerException);
//    }
//
//    [DoesNotReturn]
//    [StackTraceHidden]
//    public static void Throw<T>(
//        scoped text input,
//        string? info = null,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        throw Create<T>(input, info, innerException);
//    }
//
//    [StackTraceHidden]
//    public static ParseException Create<T>(string? input)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var message = $"ParseException - Could not parse \"{input}\" into a {typeof(T)} value";
//        return new ParseException(input, typeof(T), message);
//    }
//
//    [StackTraceHidden]
//    public static ParseException Create<T>(
//        string? input,
//        string? info,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        DefaultInterpolatedStringHandler builder = $"ParseException - Could not parse \"{input}\" into a {typeof(T)} value";
//        if (!string.IsNullOrEmpty(info))
//        {
//            builder.AppendLiteral(" - ");
//            builder.AppendLiteral(info!);
//        }
//        var message = builder.ToStringAndClear();
//
//        return new ParseException(input, typeof(T), message, innerException);
//    }
//
//    [StackTraceHidden]
//    public static ParseException Create<T>(
//        string? input,
//        KeyValues? data)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var message = $"ParseException - Could not parse \"{input}\" into a {typeof(T)} value";
//        var ex = new ParseException(input, typeof(T), message);
//        ex.Data.SetMany(data);
//        return ex;
//    }
//
//
//    [StackTraceHidden]
//    public static ParseException Create<T>(
//        scoped text input,
//        string? info = null,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => Create<T>(input.ToString(), info, innerException);
//
//    [StackTraceHidden]
//    public static ParseException Create<T>(
//        scoped text input,
//        KeyValues? data)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => Create<T>(input.ToString(), data);
//#endregion
//
//
//    public string? InputString { get; }
//
//    public Type? DestinationType { get; }
//
//    public override string Message => ExceptionAccess.RefMessageField(this) ?? "";
//
//    public ParseException(string? inputString, Type? destinationType, string? message = null, Exception? innerException = null)
//        : base(message, innerException)
//    {
//        this.InputString = inputString;
//        this.DestinationType = destinationType;
//        try
//        {
//            ref string? m = ref ExceptionAccess.RefMessageField(this);
//            m = message;
//        }
//        catch (Exception ex)
//        {
//            Debugger.Break();
//            throw;
//        }
//
//    }
//}