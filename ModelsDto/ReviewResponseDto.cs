namespace marketplace_api.ModelsDto;

public class ReviewResponseDto
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int ProductEvaluation { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
    public string Description { get; set; }
}
