using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ScrubJay.Functional.Asp;

/// <summary>
/// Extensions on <see cref="Result{T}"/>
/// </summary>
public static class ResultTExtensions
{
    extension<T>(Result<T>)
    {
        public static Result<T> FromIActionResult(IActionResult iActionResult)
        {
            if (iActionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is T value)
                {
                    if (objectResult.StatusCode == StatusCodes.Status200OK)
                    {
                        return Result<T>.Ok(value);
                    }
                    return new ProblemException($"IActionResult Error: {objectResult.Value}");
                }
                else if (objectResult.Value is Exception exception)
                {
                    return Result<T>.Error(exception);
                }
                else if (objectResult.Value is ProblemDetails problemDetails)
                {
                    return Result<T>.Error(new ProblemException(problemDetails.ToProblem()));
                }
            }
            return new ProblemException($"IActionResult Error: {iActionResult}");
        }

        public static Result<T> FromActionResult(ActionResult actionResult)
        {
            if (actionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is T value)
                {
                    if (objectResult.StatusCode == StatusCodes.Status200OK)
                    {
                        return Result<T>.Ok(value);
                    }
                    return new ProblemException($"ActionResult Error: {objectResult.Value}");
                }
                else if (objectResult.Value is Exception exception)
                {
                    return Result<T>.Error(exception);
                }
                else if (objectResult.Value is ProblemDetails problemDetails)
                {
                    return Result<T>.Error(new ProblemException(problemDetails.ToProblem()));
                }
            }
            return new ProblemException($"ActionResult Error: {actionResult}");
        }
        
        public static Result<T> FromActionResult(ActionResult<T> actionResult)
        {
            T? value = actionResult.Value;
            var inner = actionResult.Result;
            if (inner is IStatusCodeActionResult statusCodeActionResult)
            {
                if (statusCodeActionResult.StatusCode == StatusCodes.Status200OK)
                {
                    return Result<T>.Ok(value);
                }
            }

            return Result<T>.FromActionResult(inner);
        }

    }
    
    
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Converts this <see cref="Result{T}"/> into an <see cref="IActionResult"/>
        /// </summary>
        public IActionResult ToIActionResult()
        {
            if (result.IsOk(out var value, out var error))
            {
                if (value is IStatusCodeActionResult iscar)
                    return new StatusCodeResult(iscar.StatusCode ?? ProblemDetailsHelper.DefaultOkStatusCode);
                if (value is IActionResult iar)
                    return iar;
                return new OkObjectResult(value);
            }
            else
            {
                var problem = ProblemDetailsHelper.GetProblemDetails(error);
                return new ObjectResult(problem)
                {
                    StatusCode = problem.Status ?? ProblemDetailsHelper.DefaultFailStatusCode,
                };
            }
        }
        
        /// <summary>
        /// Converts this <see cref="Result{T}"/> into an <see cref="ActionResult{T}"/>
        /// </summary>
        public ActionResult<T> ToActionResult()
        {
            if (result.IsOk(out var value, out var error))
            {
                if (value is ActionResult<T> actionResult)
                    return actionResult;
                return new ActionResult<T>(value);
            }
            else
            {
                var problem = ProblemDetailsHelper.GetProblemDetails(error);
                return new ObjectResult(problem)
                {
                    StatusCode = problem.Status ?? ProblemDetailsHelper.DefaultFailStatusCode,
                };
            }
        }
        
        /// <summary>
        /// Converts this <see cref="Result{T}"/> into an <see cref="IResult"/>
        /// </summary>
        public IResult ToIResult()
        {
            if (result.IsOk(out var value, out var error))
            {
                if (value is IResult ir)
                    return ir;
                return Results.Ok(value);
            }
            else
            {
                var problem = ProblemDetailsHelper.GetProblemDetails(error);
                return Results.Problem(problem);
            }
        }
    }
}