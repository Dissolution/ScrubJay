using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ScrubJay.Exceptions.Asp.Extensions;

[PublicAPI]
public static class AspResultExtensions
{
    extension(IActionResult? iar)
    {
        public int? StatusCode
        {
            get
            {
                if (iar is IStatusCodeActionResult statusCodeActionResult)
                    return statusCodeActionResult.StatusCode;

                return null;
            }
        }

        public Result ToResult()
        {
            Throw.IfNull(iar);

            int? statusCode = iar.StatusCode;

            // we do not need an ok value
            if (StatusCodes.IsSuccessful(statusCode))
                return Result.Ok;

            if (iar is ObjectResult objectResult)
            {
                var value = objectResult.Value;
                if (value is Result result)
                    return result;
                if (value is Exception exception)
                    return exception;
                if (value is ProblemDetails problemDetails)
                    return problemDetails.ToResult();
            }

            // Assume this is an error
            return new Exception(R($"IActionResult Error: {iar:@}"));
        }

        public Result<T> ToResult<T>()
        {
            Throw.IfNull(iar);

            int? statusCode = iar.StatusCode;

            if (iar is ObjectResult objectResult)
            {
                var value = objectResult.Value;
                if (value is Result<T> result)
                    return result;
                if (value is Exception exception)
                    return exception;
                if (value is ProblemDetails problemDetails)
                    return problemDetails.ToResult<T>();
                if (StatusCodes.IsSuccessful(statusCode))
                {
                    if (value is T ok)
                        return Result<T>.Ok(ok);
                    // we have an okay value, but it cannot be a T
                    throw new InvalidOperationException(R($"Ok IActionResult Value '{value:@}' cannot be cast to a {typeof(T):@} Result"));
                }
            }

            // Assume this is an error
            return new Exception(R($"IActionResult Error: {iar:@}"));
        }
    }

    extension<T>(ActionResult<T>? art)
    {
        public int? StatusCode
        {
            get
            {
                if (art is not null)
                {
                    if (art.Result is not null)
                    {
                        return art.Result.StatusCode;
                    }

                    if (art.Value is not null)
                    {
                        return 200; // convention
                    }
                }
                return null;
            }
        }

        public Result<T> ToResult()
        {
            Throw.IfNull(art);

            T? value = art.Value;
            ActionResult? inner = art.Result;

            if (inner is null)
            {
                return Result<T>.Ok(value!); // has value == no inner == is success
            }

            return inner.ToResult<T>();
        }
    }

#if NET7_0_OR_GREATER
    extension(IResult? ir)
    {
        public int? StatusCode
        {
            get
            {
                if (ir is IStatusCodeHttpResult statusCodeHttpResult)
                {
                    return statusCodeHttpResult.StatusCode;
                }

                return null;
            }
        }

        public Result ToResult()
        {
            Throw.IfNull(ir);
            
            if (ir is IValueHttpResult valueHttpResult)
            {
                var value = valueHttpResult.Value;
                if (value is Result result)
                    return result;
                if (value is Exception exception)
                    return exception;
                if (value is ProblemDetails problemDetails)
                    return problemDetails.ToResult();
            }

            if (ir is IStatusCodeHttpResult statusCodeHttpResult)
            {
                if (StatusCodes.IsSuccessful(statusCodeHttpResult.StatusCode))
                {
                    return Result.Ok;
                }
            }

            // Assume this is an error
            return new Exception(R($"IResult Error: {ir:@}"));
        }
        
        public Result<T> ToResult<T>()
        {
            Throw.IfNull(ir);

            int? statusCode = (ir is IStatusCodeHttpResult statusResult) ? statusResult.StatusCode : null;
            
            if (ir is IValueHttpResult valueHttpResult)
            {
                var value = valueHttpResult.Value;
                if (value is Result<T> result)
                    return result;
                if (value is Exception exception)
                    return exception;
                if (value is ProblemDetails problemDetails)
                    return problemDetails.ToResult<T>();
                if (StatusCodes.IsSuccessful(statusCode))
                {
                    if (value is T ok)
                        return ok;
                    // we have an okay value, but it cannot be a T
                    throw new InvalidOperationException(R($"Ok IResult Value '{value:@}' cannot be cast to a {typeof(T):@} Result"));
                }
            }

            // Assume this is an error
            return new Exception(R($"IResult Error: {ir:@}"));
        }
    }
#endif
}