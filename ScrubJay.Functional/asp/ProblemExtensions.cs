using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Functional.Asp;

/// <summary>
/// Extensions on <see cref="Problem"/> related to ASP
/// </summary>
[PublicAPI]
public static class ProblemExtensions
{
    extension(ProblemDetails problemDetails)
    {
        /// <summary>
        /// Convert this <see cref="ProblemDetails"/> into a <see cref="Problem"/>
        /// </summary>
        public Problem ToProblem()
        {
            var problem = new Problem
            {
                Details = problemDetails.Detail,
                Title = problemDetails.Title,
            };
            
            foreach (var kvp in problemDetails.Extensions)
            {
                problem.Data[kvp.Key] = kvp.Value;
            }

            if (problemDetails.Status.HasValue)
            {
                problem["StatusCode"] = problemDetails.Status;
            }

            if (problemDetails.Instance is not null)
            {
                problem["Instance"] = problemDetails.Instance;
            }

            return problem;
        }
    }
    
    extension(Problem problem)
    {
        public Problem WithStatusCode(int statusCode)
        {
            problem.Data["StatusCode"] = statusCode;
            return problem;
        }
        
        /// <summary>
        /// Converts this <see cref="Problem"/> into a <see cref="ProblemDetails"/> instance
        /// </summary>
        public ProblemDetails ToProblemDetails()
        {
            // Try to extract a status code from Data
            int statusCode;
            if (problem.Data.TryGetValue("StatusCode", out var status) && status is int)
            {
                statusCode = (int)status;
            }
            else
            {
                // fallback to specified default
                statusCode = ProblemDetailsHelper.GetHttpStatusCode(problem.Exception);
            }

            string? instance = null;
            if (problem.Data.TryGetValue("Instance", out var instanceObj))
            {
                instance = instanceObj?.ToString();
            }

            var problemDetails = new ProblemDetails()
            {
                Type = problem.Exception?.GetType().Name,
                Title = problem.Title,
                Status = statusCode,
                Detail = problem.Details,
                Instance = instance,
            };

            var stackTrace = ProblemDetailsHelper.GetStackTrace(problem.Exception);
            if (stackTrace is not null)
            {
                problemDetails.Extensions["StackTrace"] = stackTrace;
            }

            foreach (var kvp in problem.Data)
            {
                if (kvp.Key.Equals("Instance", StringComparison.OrdinalIgnoreCase) ||
                    kvp.Key.Equals("StatusCode", StringComparison.OrdinalIgnoreCase))
                    continue;
                problemDetails.Extensions[kvp.Key] = kvp.Value;
            }

            return problemDetails;
        }
    }
}