using marketplace_api.Models;
using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace marketplace_api.CustomFilter;

public class CustomRoleActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Cookies.TryGetValue("token", out var jwtToken))
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
                if (roleClaim != null)
                {
                    context.HttpContext.Response.Cookies
                        .Append("role", roleClaim.Value, new CookieOptions() { HttpOnly = true, Path = "/" });
                }
                else
                {
                    context.HttpContext.Response.Cookies
                        .Append("role", "anonymous", new CookieOptions() { HttpOnly = true, Path = "/" });
                }
            }
            catch
            {
                context.HttpContext.Response.Cookies
                    .Append("role", "anonymous", new CookieOptions() { HttpOnly = true, Path = "/" });
            }
        }
        else
        {
            context.HttpContext.Response.Cookies
                .Append("role", "anonymous", new CookieOptions() { HttpOnly = true, Path = "/" });
        }

        await next();
    }
}
