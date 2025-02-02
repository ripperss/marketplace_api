using marketplace_api.CustomExeption;

namespace marketplace_api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next?.Invoke(context);
        }
        catch (NotFoundExeption ex)
        {
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
