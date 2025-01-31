using marketplace_api.Models;
using marketplace_api.ModelsDto;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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

    
}
 