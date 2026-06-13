#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#endif
using Microsoft.AspNetCore.Mvc;
using ScrubJay.Errors.Extensions;
using ScrubJay.Errors.Problems;
using ScrubJay.Errors.Validation;

namespace ScrubJay.Errors.Asp.Extensions;

[PublicAPI]
public static class ProblemDetailsExtensions
{
    extension(ProblemDetails problemDetails)
    {
        public Exception ToException()
        {
            Demand.NotNull(problemDetails);

            // Check to see if we have an Exception Type
            Type exType = ExceptionUrn.FromTypeUrn(problemDetails.Type)
                ?? typeof(Exception);

            // Construct that exception
            string? message = problemDetails.Detail ?? problemDetails.Title;
            Exception ex = Activator.TryCreateInstance<Exception>(exType, [message]).OkOr(new InvalidOperationException(message));

            // Add properties
            ex.Type = problemDetails.Type;
            ex.Title = problemDetails.Title;
            ex.Status = problemDetails.Status;
            ex.Detail = problemDetails.Detail;
            ex.Instance = problemDetails.Instance;

            // Add data
            foreach (var entry in problemDetails.Extensions)
            {
                ex.Data[entry.Key] = entry.Value;
            }

            // fin
            return ex;
        }

        public Result ToResult()
        {
            var ex = problemDetails.ToException();
            return ex;
        }

        public Result<T> ToResult<T>()
        {
            var ex = problemDetails.ToException();
            return ex;
        }

        public ActionResult ToActionResult()
        {
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
            };
        }

#if NET7_0_OR_GREATER
        public IResult ToHttpResult()
        {
            return TypedResults.Problem(problemDetails);
        }
#endif
    }
}