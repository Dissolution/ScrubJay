using System.Data.Common;
using System.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ScrubJay.Text.Collections;

namespace ScrubJay.Exceptions.Asp;

#if NET8_0_OR_GREATER
[PublicAPI]
public sealed class SJExceptionHandler : IExceptionHandler
{
    private readonly StackTraceLevel _stackTraceLevel;

    public SJExceptionHandler(StackTraceLevel stackTraceLevel)
    {
        _stackTraceLevel = stackTraceLevel;
        ProblemDetailsHelper.StackTraceLevel = stackTraceLevel;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        var problem = ProblemDetailsHelper.ToProblemDetails(exception);
        httpContext.Response.StatusCode = problem.Status ?? ProblemDetailsHelper.DefaultErrorStatusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
#endif