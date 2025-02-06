
using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomSessiontokenResousreFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        
        if (!context.HttpContext.Request.Cookies.ContainsKey("sessionToken"))
        {
            var value = Guid.NewGuid();
            context.HttpContext.Response.Cookies.Append("sessionToken", value.ToString());
        }
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        
    }
}
