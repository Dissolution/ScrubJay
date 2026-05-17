using System.Data.Common;
using System.Reflection;
using System.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrubJay.Errors.Problems;
using ScrubJay.Errors.Validation;
using ScrubJay.Text.Collections;
using ScrubJay.Universal.Extensions;
using Any = ScrubJay.Universal.Any;

namespace ScrubJay.Errors.Asp;

[PublicAPI]
public static class ProblemDetailsConverter
{
    /// <summary>
    /// A map of Error <see cref="Type">Types</see> to HTTP Status Codes.
    /// </summary>
    public static TypeMap<int> ErrorTypeToHttpStatusCodeMap { get; } = new()
    {
        [typeof(ArgumentNullException)] = StatusCodes.Status400BadRequest,
        [typeof(ArgumentException)] = StatusCodes.Status400BadRequest,
        [typeof(InvalidOperationException)] = StatusCodes.Status400BadRequest, // could be 500
        [typeof(FormatException)] = StatusCodes.Status400BadRequest,
        [typeof(ArithmeticException)] = StatusCodes.Status400BadRequest,
        [typeof(AuthenticationException)] = StatusCodes.Status401Unauthorized,
        [typeof(UnauthorizedAccessException)] = StatusCodes.Status403Forbidden,
        [typeof(SecurityException)] = StatusCodes.Status403Forbidden,
        [typeof(KeyNotFoundException)] = StatusCodes.Status404NotFound,
        [typeof(FileNotFoundException)] = StatusCodes.Status404NotFound,
        [typeof(TimeoutException)] = StatusCodes.Status408RequestTimeout,
#if NET8_0_OR_GREATER
        [typeof(OperationCanceledException)] = StatusCodes.Status499ClientClosedRequest,
        [typeof(TaskCanceledException)] = StatusCodes.Status499ClientClosedRequest,
#else
        [typeof(OperationCanceledException)] = 499,
        [typeof(TaskCanceledException)] = 499,
#endif
        [typeof(NullReferenceException)] = StatusCodes.Status500InternalServerError,
        [typeof(NotSupportedException)] = StatusCodes.Status501NotImplemented,
        [typeof(NotImplementedException)] = StatusCodes.Status501NotImplemented,
        [typeof(HttpRequestException)] = StatusCodes.Status502BadGateway,
        [typeof(IOException)] = StatusCodes.Status503ServiceUnavailable,
        [typeof(DbException)] = StatusCodes.Status503ServiceUnavailable,
        [typeof(OutOfMemoryException)] = StatusCodes.Status503ServiceUnavailable,
    };

    /// <summary>
    /// Gets or sets the default HTTP Status Code returned for Errors when their <see cref="Type"/> is not in <see cref="ErrorTypeToHttpStatusCodeMap"/>.
    /// </summary>
    public static int DefaultErrorStatusCode
    {
        get;
        set => field = Validate.InRange(value, 100, 600).OkOr(StatusCodes.Status500InternalServerError);
    } = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// Gets or sets the default level of StackTrace that appears in generated <see cref="ProblemDetails"/>.
    /// </summary>
    public static StackTraceLevel StackTraceLevel { get; set; } = StackTraceLevel.None;

   


    private static string GetSanitizedStackTrace(Exception exception)
    {
        Debugger.Break();
        throw new NotImplementedException();
    }

    private static string GetFilesAndMethodsStackTrace(Exception exception)
    {
        var trace = new StackTrace(exception, true);
        var frames = trace.GetFrames();

        string? lastFileName = null;
        bool indented = false;

        return TextBuilder.Rent()
            .Delimit(TB.NewLine, frames, (tb, frame) =>
            {
                var fileName = frame.GetFileName();
                if (fileName != lastFileName)
                {
                    if (indented)
                    {
                        tb.Outdent();
                    }
                    tb.Append(fileName).Append(":").Indent().NewLine();
                    lastFileName = fileName;
                    indented = true;
                }

                tb.Append(frame.GetFileLineNumber())
                    .Append(':')
                    .Append(frame.GetFileColumnNumber())
                    .Append(" - ");

                var method = frame.GetMethod();
                tb.IfNotNull(method, TB.Render, TB.Write("<unknown method>"));
            })
            .If(indented, tb => tb.Outdent())
            .ToStringAndDispose();
    }

    public static string? FixStackTrace(Exception? exception)
    {
        if (exception is null)
            return null;
        return StackTraceLevel switch
        {
            StackTraceLevel.None => null,
            StackTraceLevel.Sanitized => GetSanitizedStackTrace(exception),
            StackTraceLevel.FilesAndMethods => GetFilesAndMethodsStackTrace(exception),
            StackTraceLevel.Full => exception.StackTrace,
            _ => null,
        };
    }

    private static IDictionary<string, object?> DataToExtensions(IDictionary data)
    {
        var dict = new Dictionary<string, object?>(capacity: data.Count, StringComparer.Ordinal);

        foreach (DictionaryEntry entry in data)
        {
            string key = entry.Key!.ToString()!;
            if (key is ProblemDetailsExtensions.TYPE_PROPERTY
                or ProblemDetailsExtensions.TITLE_PROPERTY
                or ProblemDetailsExtensions.STATUS_PROPERTY
                or ProblemDetailsExtensions.DETAIL_PROPERTY
                or ProblemDetailsExtensions.INSTANCE_PROPERTY)
            {
                continue;
            }
            dict[key] = entry.Value;
        }

        return dict;
    }

   
    public static int GetHttpStatusCode(Exception? exception)
    {
        Type exceptionType = Any.GetType<Exception>(exception);
        int statusCode = ErrorTypeToHttpStatusCodeMap.GetValueOrDefault(exceptionType, DefaultErrorStatusCode);
        return statusCode;
    }
    
    public static int GetHttpStatusCode<E>(in E? error)
    {
        Type errorType = Any.GetType(in error);
        int statusCode = ErrorTypeToHttpStatusCodeMap.GetValueOrDefault(errorType, DefaultErrorStatusCode);
        return statusCode;
    }
    
    public static ProblemDetails ToProblemDetails(Exception exception)
    {
        var extensions = DataToExtensions(exception.Data);
        string? stackTrace = FixStackTrace(exception);
        extensions["StackTrace"] = stackTrace;

        var problem = new ProblemDetails()
        {
            Type = exception.Type,
            Title = exception.Title,
            Status = exception.Status ?? GetHttpStatusCode(exception),
            Detail = exception.Detail,
            Instance = exception.Instance,
        };
        problem.Extensions.AddMany(extensions);

        return problem;
    }

    public static ProblemDetails ToProblemDetails<E>(E? error)
    {
        if (error is ProblemDetails problemDetails)
            return problemDetails;
        if (error is Exception exception)
            return ToProblemDetails(exception);
        
        var errorType = Any.GetType<E>(in error);
        
        problemDetails = new ProblemDetails()
        {
            Type = $"urn:{errorType.Namespace}:{Type.Render(errorType)}",
            Title = "Error",
            Status = GetHttpStatusCode(error),
            Detail = error?.ToString(),
            Instance = null,
        };

        var properties = errorType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (!property.CanRead || property.GetIndexParameters().Length != 0)
                continue;

            var key = property.Name;
            if (Result.Try(() => property.GetValue(error)).IsOk(out var value))
            {
                problemDetails.Extensions[key] = value;
            }
        }

        return problemDetails;
    }
}