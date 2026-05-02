using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Exceptions.Asp.Extensions;

[PublicAPI]
public static class ControllerExtensions
{
    extension(ControllerBase controller)
    {
        public ObjectResult Problem(ProblemDetails problemDetails)
        {
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
            };
        }

        public ObjectResult Problem(Exception exception)
            => controller.Problem(exception.ToProblemDetails());


        public ActionResult FromResult(Result result)
            => result.ToActionResult();

        public ActionResult<T> FromResult<T>(Result<T> result)
            => result.ToActionResult();

        public ActionResult<T> FromResult<T, E>(Result<T, E> result)
            => result.ToActionResult();
    }
}