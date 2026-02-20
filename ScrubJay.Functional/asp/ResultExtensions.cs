using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Functional.Asp;

/// <summary>
/// Extensions on <see cref="Result"/>, <see cref="Result{T}"/>, and <see cref="Result{T,E}"/> related to ASP
/// </summary>
[PublicAPI]
public static partial class ResultExtensions
{
    extension(Result)
    {
        public static Result FromIActionResult(IActionResult actionResult)
        {
            if (actionResult is null)
                return new ArgumentNullException(nameof(actionResult));

            if (actionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is null || objectResult.Value is Unit)
                    return Result.Ok;
                if (objectResult.Value is Result result)
                    return result;
                if (objectResult.Value is Exception ex)
                    return ex;
                if (objectResult.Value is ProblemDetails problemDetails)
                    return new ProblemException(problemDetails.ToProblem());
            }

            if (actionResult is StatusCodeResult statusCodeResult
                && statusCodeResult.StatusCode == StatusCodes.Status200OK)
                return Result.Ok;

            // have to assume failure
            return new ProblemException($"IActionResult Error: {actionResult}");
        }

        public static Result FromActionResult(ActionResult actionResult)
        {
            if (actionResult is null)
                return new ArgumentNullException(nameof(actionResult));

            if (actionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is null || objectResult.Value is Unit)
                    return Result.Ok;
                if (objectResult.Value is Result result)
                    return result;
                if (objectResult.Value is Exception ex)
                    return ex;
                if (objectResult.Value is ProblemDetails problemDetails)
                    return new ProblemException(problemDetails.ToProblem());
            }

            if (actionResult is StatusCodeResult statusCodeResult
                && statusCodeResult.StatusCode == StatusCodes.Status200OK)
                return Result.Ok;

            // have to assume failure
            return new ProblemException($"IActionResult Error: {actionResult}");
        }
    }


    extension(Result result)
    {
        /// <summary>
        /// Convert this <see cref="Result"/> into an <see cref="IActionResult"/>
        /// </summary>
        public IActionResult ToIActionResult()
        {
            if (!result.IsError(out var error))
            {
                return new OkResult();
            }

            var problem = ProblemDetailsHelper.GetProblemDetails(error);
            return new ObjectResult(problem)
            {
                StatusCode = problem.Status ?? ProblemDetailsHelper.DefaultFailStatusCode,
            };
        }

        /// <summary>
        /// Convert this <see cref="Result"/> into an <see cref="ActionResult"/>
        /// </summary>
        public ActionResult ToActionResult()
        {
            if (!result.IsError(out var error))
            {
                return new OkResult();
            }

            var problem = ProblemDetailsHelper.GetProblemDetails(error);
            return new ObjectResult(problem)
            {
                StatusCode = problem.Status ?? ProblemDetailsHelper.DefaultFailStatusCode,
            };
        }

        /// <summary>
        /// Convert this <see cref="Result"/> into an <see cref="IResult"/>
        /// </summary>
        public IResult ToIResult()
        {
            if (!result.IsError(out var error))
            {
                return Results.Ok();
            }

            var problem = ProblemDetailsHelper.GetProblemDetails(error);
            return Results.Problem(problem);
        }
    }
}