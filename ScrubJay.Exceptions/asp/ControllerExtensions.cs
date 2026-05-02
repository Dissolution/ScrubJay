using Microsoft.AspNetCore.Mvc;
using ScrubJay.Functional;

namespace ScrubJay.Exceptions.Asp;

[PublicAPI]
public static class ControllerExtensions
{
    extension(ControllerBase controller)
    {
        #region ActionResult
        
        public ActionResult FromResult(Result result)
        {
            if (!result.IsError(out var exception))
            {
                return controller.Ok();
            }
            
            
        }
        
        #endregion
        
        #region ActionResult<T>
        #endregion
    }
}