using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace marketplace_api.Extenions;

public static  class AuthExestions
{
    public static IServiceCollection AddAuth(this IServiceCollection service, IConfiguration configuration)
    {

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();

         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(authSettings.Secret))
         };
         options.Events = new JwtBearerEvents
         {
             OnMessageReceived = context => 
             {
                 context.Token = context.Request.Cookies["token"];

                 return Task.CompletedTask;
             }
         };
     });
    
        service.AddScoped<JwtService>();
        service.AddScoped<IAuthenticationService, AuthenticationService>();

        service.AddAuthorization(options =>
        {
            options.AddPolicy("Caki", options =>
            {
                options.RequireClaim(ClaimTypes.Name, "Caki");                                        
            });
        });

        return service;
    }
}
