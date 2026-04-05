using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Functional.Asp.Conversion;

internal static class ResultConverter
{
    // input types: exception, Result, Result<T>, Result<T,E>, Option<T>, Problem, ProblemException, ProblemDetails
    
    public static IActionResult ToIActionResult(Exception exception)
    {
        throw new NotImplementedException();
    }

    public static ActionResult ToActionResult(Exception exception)
    {
        throw new NotImplementedException();
    }

    public static ActionResult<T> ToActionResult<T>(Exception exception)
    {
        throw new NotImplementedException();
    }

    public static IResult ToIResult(Exception exception)
    {
        throw new NotImplementedException();
    }

    public static IResult ToTypedResult(Exception exception)
    {
        throw new NotImplementedException();
    }
}

internal static class ProblemConverter
{
    public static ProblemDetails ToProblemDetails(Problem problem)
    {
        var ex = new ProblemException("blah");
    }
    
    public static ProblemDetails ToProblemDetails(ProblemException problem)
    {
        throw new NotImplementedException();
    }
    
    public static ProblemDetails ToProblemDetails(Exception problem)
    {
        throw new NotImplementedException();
    }
    
    
    public static ProblemException ToProblemException(ProblemDetails problem)
    {
        throw new NotImplementedException();
    }
    
    public static ProblemException ToProblemException(Problem problem)
    {
        throw new NotImplementedException();
    }
    
    public static ProblemException ToProblemException(Exception problem)
    {
        throw new NotImplementedException();
    }
    
    
    public static Problem ToProblem(ProblemDetails problem)
    {
        throw new NotImplementedException();
    }
    
    public static Problem ToProblem(ProblemException problem)
    {
        throw new NotImplementedException();
    }
    
    public static Problem ToProblem(Exception problem)
    {
        throw new NotImplementedException();
    }
}