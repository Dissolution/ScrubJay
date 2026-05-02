using System.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrubJay.Text.Building;
using ScrubJay.Universal;

namespace ScrubJay.Exceptions.Asp;

[PublicAPI]
public static class ProblemDetailsHelper
{
    private static readonly Dictionary<Type, int> _exceptionStatusCodes;
    
    
    public static int DefaultOkStatusCode
    {
        get => field;
        set => field = ValidateHttpStatusCode(value);
    } = StatusCodes.Status200OK;

    public static int DefaultErrorStatusCode
    {
        get => field;
        set => field = ValidateHttpStatusCode(value);
    } = StatusCodes.Status500InternalServerError;

    public static StackTraceLevel StackTraceLevel { get; set; } = StackTraceLevel.None;

    static ProblemDetailsHelper()
    {
        _exceptionStatusCodes = new Dictionary<Type, int>
        {
            [typeof(ArgumentNullException)] = StatusCodes.Status400BadRequest,
            [typeof(ArgumentException)] = StatusCodes.Status400BadRequest,
            [typeof(InvalidOperationException)] = StatusCodes.Status400BadRequest,
            [typeof(FormatException)] = StatusCodes.Status400BadRequest,
            [typeof(AuthenticationException)] = StatusCodes.Status401Unauthorized,
            [typeof(UnauthorizedAccessException)] = StatusCodes.Status403Forbidden,
            [typeof(SecurityException)] = StatusCodes.Status403Forbidden,
            [typeof(KeyNotFoundException)] = StatusCodes.Status404NotFound,
            [typeof(FileNotFoundException)] = StatusCodes.Status404NotFound,
            [typeof(NotSupportedException)] = StatusCodes.Status405MethodNotAllowed,
            [typeof(TimeoutException)] = StatusCodes.Status408RequestTimeout,

#if NET8_0_OR_GREATER
            [typeof(OperationCanceledException)] = StatusCodes.Status499ClientClosedRequest,
            [typeof(TaskCanceledException)] = StatusCodes.Status499ClientClosedRequest,
#else
            [typeof(OperationCanceledException)] = 499,
            [typeof(TaskCanceledException)] = 499,
#endif
            [typeof(NullReferenceException)] = StatusCodes.Status500InternalServerError,
        };
    }

    private static int ValidateHttpStatusCode(int code)
    {
        if (code < 100 || code > 599)
            throw new ArgumentOutOfRangeException();
        return code;
    }
    
    
    
    private static string GetFilesAndMethodsStackTrace(Exception error)
    {
        var trace = new StackTrace(error, true);
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

    
    
    public static void SetDefaultStatusCode<E>(int statusCode)
        where E : Exception
    {
        _exceptionStatusCodes[typeof(E)] = ValidateHttpStatusCode(statusCode);
    }
    
    public static void SetDefaultStatusCode(Type exceptionType, int statusCode)
    {
        if (!exceptionType.IsAssignableTo(typeof(Exception)))
            throw new ArgumentException();
        
        _exceptionStatusCodes[exceptionType] = ValidateHttpStatusCode(statusCode);;
    }

    public static int GetHttpStatusCode(Exception? exception)
    {
        Type exceptionType = exception?.GetType() ?? typeof(Exception);
        int statusCode = _exceptionStatusCodes.GetValueOrDefault(exceptionType, DefaultErrorStatusCode);
        return statusCode;
    }

    public static string? GetStackTrace(Exception? exception)
    {
        if (exception is null)
            return null;
        return StackTraceLevel switch
        {
            StackTraceLevel.FilesAndMethods => GetFilesAndMethodsStackTrace(exception),
            StackTraceLevel.Full => exception.StackTrace,
            _ => null,
        };
    }

    public static Problem GetProblem<E>(E? error)
    {
        return error switch
        {
            null => new Problem()
            {
                Title = "Error",
                Data =
                {
                    { "Type", Any.GetType<E>(in error).Name },
                },
            },
            Problem problem => problem,
            ProblemDetails problemDetails => problemDetails.ToProblem(),
            _ => new Problem()
            {
                Title = "Error",
                Details = Any.ToString(in error),
                Data =
                {
                    { "Type", Any.GetType<E>(in error).Name },
                },
            },
        };
    }


    public static ProblemDetails GetProblemDetails(Exception? exception)
    {
        Type exceptionType = exception?.GetType() ?? typeof(Exception);
        ProblemDetails problem;

        if (exception is null)
        {
            problem = new()
            {
                Type = exceptionType.Name,
                Title = "Unknown Error",
                Detail = "Unknown Error",
                Status = DefaultErrorStatusCode,
            };
        }
        else
        {
            int statusCode = _exceptionStatusCodes.GetValueOrDefault(exceptionType, DefaultErrorStatusCode);

            string? detail = StackTraceLevel switch
            {
                StackTraceLevel.FilesAndMethods => GetFilesAndMethodsStackTrace(exception),
                StackTraceLevel.Full => exception.StackTrace,
                _ => null,
            };

            string? instance = null;
            if (!string.IsNullOrEmpty(exception.HelpLink) &&
                Uri.TryCreate(exception.HelpLink, UriKind.Absolute, out var uri))
            {
                instance = uri.ToString();
            }

            problem = new ProblemDetails
            {
                Type = exceptionType.Name,
                Title = exception.Message,
                Detail = detail,
                Status = statusCode,
                Instance = instance,
            };
        }

        return problem;
    }

    public static ProblemDetails GetProblemDetails<E>([AllowNull, MaybeNull] E? error)
    {
        return error switch
        {
            null => new ProblemDetails()
            {
                Title = "Error",
                Type = typeof(E).Name,
            },
            Problem problem => problem.ToProblemDetails(),
            ProblemDetails details => details,
            _ => new ProblemDetails
            {
                Title = "Error",
                Type = Any.GetType<E>(in error).Name,
                Detail = error.ToString(),
            },
        };
    }
}