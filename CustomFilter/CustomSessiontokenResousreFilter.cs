using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomSessiontokenResousreFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        var value = new Guid();
        context.HttpContext.Response.Cookies.Append("sessionToken",value.ToString());
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        
    }
}
