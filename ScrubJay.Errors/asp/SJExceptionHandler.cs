#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ScrubJay.Errors.Asp;

[PublicAPI]
public sealed class SJExceptionHandler : IExceptionHandler
{
    private readonly StackTraceLevel _stackTraceLevel;

    public SJExceptionHandler(StackTraceLevel stackTraceLevel)
    {
        _stackTraceLevel = stackTraceLevel;
        ProblemDetailsConverter.StackTraceLevel = stackTraceLevel;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        var problem = ProblemDetailsConverter.ToProblemDetails(exception);
        httpContext.Response.StatusCode = problem.Status ?? ProblemDetailsConverter.DefaultErrorStatusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
#endif