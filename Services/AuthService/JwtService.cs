using marketplace_api.CustomExeption;
using marketplace_api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace marketplace_api.Services.AuthService;

public class JwtService
{
    private readonly IOptions<AuthSettings> _options;

    public JwtService(IOptions<AuthSettings> options)
    {
        _options = options;
    }
    
    public string GenerateJwtToken(User user)
    {
        List<Claim> claim = [
            new Claim(ClaimTypes.Role,user.Role.ToString()),
            new Claim("Id",user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.Name)
            ];
                        
        var jwtToken = new JwtSecurityToken
            (
            expires: DateTime.UtcNow.Add(_options.Value.Expires),
            claims: claim,
            signingCredentials: 
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret)),
                SecurityAlgorithms.HmacSha256));

        return new  JwtSecurityTokenHandler().WriteToken(jwtToken);
    }


    public int GetIdUser(HttpContext context)
    {
        var token = context.Request.Cookies["token"];
        if (string.IsNullOrEmpty(token))
        {
            throw new NotFoundExeption("Пользователь не зарегистрирован"); 
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Claim userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Id"); 

        if (userIdClaim == null)
        {
            throw new SecurityTokenException("Claim 'Id' не найден в токене."); 
        }

        if (int.TryParse(userIdClaim.Value, out int userId)) 
        {
            return userId;
        }
        else
        {
            throw new FormatException("Невозможно преобразовать значение claim'а 'Id' в целое число."); 
        }
    }

}
 