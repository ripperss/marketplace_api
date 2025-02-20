using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.Services.ReviewService;

public interface IReviewService
{
    Task<List<ReviewResponseDto>> GetAllProductReviewsAsync(int productId);
    Task<List<ReviewResponseDto>> GetAllUserReviewsAsync(int userId);
    Task<ReviewResponseDto> GetReviewsAsync(int rewiewid);
    Task CreateRewiewAsync(ReviewRequestDto reviewDto);
    Task DeleteRewiewAsync(int rewiewId);
    Task UpdateRewiewAsync(ReviewRequestDto newReviewDto, int reviewId);
}
