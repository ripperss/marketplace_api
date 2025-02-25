using Microsoft.Identity.Client;

namespace marketplace_api.ModelsDto;

public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }

}