using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Exceptions.Asp.Extensions;

[PublicAPI]
public static class ExceptionExtensions
{
    extension<E>(E exception)
        where E : Exception
    {
        public ProblemDetails ToProblemDetails() => ProblemDetailsHelper.ToProblemDetails(exception);
    }
}