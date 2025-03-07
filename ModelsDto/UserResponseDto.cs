using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public Role Role { get; set; }
}
