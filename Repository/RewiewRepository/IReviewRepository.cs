using marketplace_api.Models;

namespace marketplace_api.Repository.Rewiew;

public interface IReviewRepository
{
    Task<List<Review>> GetAllProductReviewsAsync(int productId);
    Task<List<Review>> GetAllUserReviewsAsync(int userId);
    Task<Review> GetReviewsAsync(int rewiewid);
    Task CreateRewiewAsync(Review review);
    Task DeleteRewiewAsync(int rewiewId);
    Task UpdateRewiewAsync(Review newReview, int reviewId);
}
