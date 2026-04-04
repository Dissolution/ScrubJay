using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ScrubJay.Universal;

namespace ScrubJay.Functional.Asp;

[PublicAPI]
public static class ActionResultExtensions
{
    extension(IActionResult actionResult)
    {
        public Result<T, Problem> ToProblemResult<T>()
        {
            if (actionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is T value && objectResult.StatusCode == StatusCodes.Status200OK)
                {
                    return Result<T, Problem>.Ok(value);
                }

                if (objectResult.Value is Exception exception)
                {
                    return Result<T, Problem>.Error(exception);
                }

                if (objectResult.Value is ProblemDetails problemDetails)
                {
                    return Result<T, Problem>.Error(problemDetails.ToProblem());
                }

                if (objectResult.Value is Problem problem)
                {
                    return Result<T, Problem>.Error(problem);
                }
            }
            
            if (actionResult is IStatusCodeActionResult statusCodeResult 
                && statusCodeResult.StatusCode == StatusCodes.Status200OK)
            {
                return Result<T, Problem>.Ok(default!);
            }

            return Result<T, Problem>.Error(new Problem($"(IActionResult){typeof(T)} error: {actionResult}"));
        }
    }
}