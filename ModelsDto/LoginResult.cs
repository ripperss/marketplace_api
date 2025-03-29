using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class LoginResult
{
    public string Name { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }
    public int AuthResult {  get; set; }
}
