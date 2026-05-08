using Microsoft.AspNetCore.Http;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Errors.Asp.Extensions;

[PublicAPI]
public enum StatusCodeClass
{
    Unknown = 0,
    Informational = 1,
    Successful = 2,
    Redirection = 3,
    ClientError = 4,
    ServerError = 5,
}

[PublicAPI]
public static class StatusCodesExtensions
{
    extension(StatusCodes)
    {
        public static StatusCodeClass GetStatusCodeClass(int statusCode) => statusCode switch
        {
            >= 100 and <= 199 => StatusCodeClass.Informational,
            >= 200 and <= 299 => StatusCodeClass.Successful,
            >= 300 and <= 399 => StatusCodeClass.Redirection,
            >= 400 and <= 499 => StatusCodeClass.ClientError,
            >= 500 and <= 599 => StatusCodeClass.ServerError,
            _ => StatusCodeClass.Unknown,
        };

        public static StatusCodeClass GetStatusCodeClass(int? statusCode)
        {
            if (statusCode.TryGetValue(out int code))
                return StatusCodes.GetStatusCodeClass(code);
            return StatusCodeClass.Unknown;
        }

        public static bool IsSuccessful(int statusCode) 
            => StatusCodes.GetStatusCodeClass(statusCode) == StatusCodeClass.Successful;
        
        public static bool IsSuccessful(int? statusCode) 
            => StatusCodes.GetStatusCodeClass(statusCode) == StatusCodeClass.Successful;
    }
}