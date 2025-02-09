using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomCartActionFilter : IActionFilter
{
    
    public void OnActionExecuted(ActionExecutedContext context)
    {
       
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      
    }
}
