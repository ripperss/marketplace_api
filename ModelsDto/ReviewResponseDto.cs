using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class ReviewResponseDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public UserDto User { get; set; }
    public int ProductId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int ProductEvaluation { get; set; }
    public int Rating { get; set; }
}
