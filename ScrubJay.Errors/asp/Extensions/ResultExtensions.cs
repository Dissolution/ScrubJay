#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#endif
using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Errors.Asp.Extensions;

[PublicAPI]
public static class ResultExtensions
{
    extension(Result result)
    {
        public ActionResult ToActionResult()
        {
            if (result.IsError(out var exception))
            {
                return exception.ToProblemDetails().ToActionResult();
            }
            return new OkResult();
        }

#if NET7_0_OR_GREATER
        public IResult ToHttpResult()
        {
            if (result.IsError(out var exception))
            {
                return exception.ToProblemDetails().ToHttpResult();
            }
            return TypedResults.Ok();
        }
#endif
    }

    extension<T>(Result<T> result)
    {
        public ActionResult<T> ToActionResult()
        {
            if (result.IsOk(out var ok, out var exception))
            {
                if (ok is ActionResult<T> art)
                    return art;
                if (ok is ActionResult ar)
                    return ar;
                if (ok is ProblemDetails problem)
                    return problem.ToActionResult();
                return new ActionResult<T>(ok);
            }

            return exception.ToProblemDetails().ToActionResult();
        }

#if NET7_0_OR_GREATER
        public IResult ToHttpResult()
        {
            if (result.IsOk(out var ok, out var exception))
            {
                if (ok is IResult ir)
                    return ir;
                if (ok is ProblemDetails problem)
                    return problem.ToHttpResult();
                return TypedResults.Ok<T>(ok);
            }

            return exception.ToProblemDetails().ToHttpResult();
        }
#endif
    }

    extension<T, E>(Result<T, E> result)
    {
        public ActionResult<T> ToActionResult()
        {
            if (result.IsOk(out var ok, out var error))
            {
                if (ok is ActionResult<T> art)
                    return art;
                if (ok is ActionResult ar)
                    return ar;
                if (ok is ProblemDetails problem)
                    return problem.ToActionResult();
                return new ActionResult<T>(ok);
            }
            else
            {
                if (error is ActionResult<T> art)
                    return art;
                if (error is ActionResult ar)
                    return ar;
                if (error is ProblemDetails problemDetails)
                    return problemDetails.ToActionResult();
                var problem = ProblemDetailsConverter.ToProblemDetails(error);
                return problem.ToActionResult();
            }
        }

#if NET7_0_OR_GREATER
        public IResult ToHttpResult()
        {
            if (result.IsOk(out var ok, out var error))
            {
                if (ok is IResult ir)
                    return ir;
                if (ok is ProblemDetails problem)
                    return problem.ToHttpResult();
                return TypedResults.Ok<T>(ok);
            }
            else
            {
                if (error is IResult ir)
                    return ir;
                if (error is ProblemDetails problemDetails)
                    return problemDetails.ToHttpResult();
                var problem = ProblemDetailsConverter.ToProblemDetails(error);
                return problem.ToHttpResult();
            }
        }
#endif
    }
}