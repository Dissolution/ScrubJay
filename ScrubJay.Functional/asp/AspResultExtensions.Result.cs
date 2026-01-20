using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Functional.Asp;

/// <summary>
/// Extensions on <see cref="Result"/>, <see cref="Result{T}"/>, and <see cref="Result{T,E}"/> related to ASP
/// </summary>
[PublicAPI]
public static partial class AspResultExtensions
{
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