using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Functional.Asp;

/// <summary>
/// Extensions on <see cref="ControllerBase"/>
/// </summary>
[PublicAPI]
public static class ControllerExtensions
{
    extension(ControllerBase controller)
    {
        /// <summary>
        /// Returns an <see cref="ActionResult"/> converted from a <see cref="Result"/>
        /// </summary>
        public ActionResult FromResult(Result result)
        {
            if (!result.IsError(out var exception))
            {
                return new OkResult();
            }

            int? statusCode = ProblemDetailsHelper.GetHttpStatusCode(exception);

            var problemDetails = controller
                .ProblemDetailsFactory
                .CreateProblemDetails(
                    httpContext: controller.HttpContext,
                    statusCode: statusCode,
                    title: exception.Message,
                    type: exception.GetType().Name,
                    detail: ProblemDetailsHelper.GetStackTrace(exception));

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode,
            };
        }

        /// <summary>
        /// Returns an <see cref="ActionResult{T}"/> converted from a <see cref="Result{T}"/>
        /// </summary>
        public ActionResult<T> FromResult<T>(Result<T> result)
        {
            if (result.IsOk(out var value, out var ex))
            {
                if (value is ActionResult<T> art)
                    return art;
                if (value is ActionResult ar)
                    return ar;
                return new ActionResult<T>(value);
            }

            var problemDetails = ProblemDetailsHelper.GetProblemDetails(ex);

#if NET9_0_OR_GREATER
            return controller.Problem(
                detail: problemDetails.Detail,
                instance: problemDetails.Instance,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type,
                extensions: problemDetails.Extensions);
#else
            var objectResult = controller.Problem(
                detail: problemDetails.Detail,
                instance: problemDetails.Instance,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type);
            if (problemDetails.Extensions.Count > 0)
            {
                var extensions = (objectResult.Value as ProblemDetails)!.Extensions;
                foreach (var ext in problemDetails.Extensions)
                {
                    extensions.Add(ext);
                }
            }

            return objectResult;
#endif
        }

        /// <summary>
        /// Returns an <see cref="ActionResult{T}"/> converted from a <see cref="Result{T,E}"/>
        /// </summary>
        public ActionResult<T> FromResult<T, E>(Result<T, E> result)
        {
            if (result.IsOk(out var value, out var error))
            {
                if (value is ActionResult<T> art)
                    return art;
                if (value is ActionResult ar)
                    return ar;
                return new ActionResult<T>(value);
            }
            else
            {
                if (error is ActionResult<T> art)
                    return art;

                if (error is ActionResult ar)
                    return ar;

                var problemDetails = ProblemDetailsHelper.GetProblemDetails<E>(error);
#if NET9_0_OR_GREATER
                return controller.Problem(
                    detail: problemDetails.Detail,
                    instance: problemDetails.Instance,
                    statusCode: problemDetails.Status,
                    title: problemDetails.Title,
                    type: problemDetails.Type,
                    extensions: problemDetails.Extensions);
#else
                var objectResult = controller.Problem(
                    detail: problemDetails.Detail,
                    instance: problemDetails.Instance,
                    statusCode: problemDetails.Status,
                    title: problemDetails.Title,
                    type: problemDetails.Type);
                if (problemDetails.Extensions.Count > 0)
                {
                    var extensions = (objectResult.Value as ProblemDetails)!.Extensions;
                    foreach (var ext in problemDetails.Extensions)
                    {
                        extensions.Add(ext);
                    }
                }

                return objectResult;
#endif
            }
        }

        public ActionResult<T> FromResult<T>(Result<T, Problem> result)
        {
            if (result.IsOk(out var value, out var problem))
            {
                if (value is ActionResult<T> art)
                    return art;
                if (value is ActionResult ar)
                    return ar;
                return new ActionResult<T>(value);
            }
            else
            {
                var problemDetails = problem.ToProblemDetails();
#if NET9_0_OR_GREATER
                return controller.Problem(
                    detail: problemDetails.Detail,
                    instance: problemDetails.Instance,
                    statusCode: problemDetails.Status,
                    title: problemDetails.Title,
                    type: problemDetails.Type,
                    extensions: problemDetails.Extensions);
#else
                var objectResult = controller.Problem(
                    detail: problemDetails.Detail,
                    instance: problemDetails.Instance,
                    statusCode: problemDetails.Status,
                    title: problemDetails.Title,
                    type: problemDetails.Type);
                if (problemDetails.Extensions.Count > 0)
                {
                    var extensions = (objectResult.Value as ProblemDetails)!.Extensions;
                    foreach (var ext in problemDetails.Extensions)
                    {
                        extensions.Add(ext);
                    }
                }

                return objectResult;
#endif
            }
        }

        public ObjectResult FromProblem(Problem problem)
        {
            var problemDetails = problem.ToProblemDetails();
#if NET9_0_OR_GREATER
            return controller.Problem(
                detail: problemDetails.Detail,
                instance: problemDetails.Instance,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type,
                extensions: problemDetails.Extensions);
#else
            var objectResult = controller.Problem(
                detail: problemDetails.Detail,
                instance: problemDetails.Instance,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type);
            if (problemDetails.Extensions.Count > 0)
            {
                var extensions = (objectResult.Value as ProblemDetails)!.Extensions;
                foreach (var ext in problemDetails.Extensions)
                {
                    extensions.Add(ext);
                }
            }

            return objectResult;
#endif
        }
    }
}