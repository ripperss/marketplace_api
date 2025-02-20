using marketplace_api.CustomExeption;
using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace marketplace_api.Repository.Rewiew;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _appDbContext;

    public ReviewRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task CreateRewiewAsync(Review review)
    {
        await _appDbContext.AddAsync(review);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteRewiewAsync(int rewiewId)
    {
        var review = await _appDbContext.Reviews.FindAsync(rewiewId);
        if (review == null)
        {
            throw new NotFoundExeption("Данный отзыв не найден");
        }

        _appDbContext.Remove(review); 
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Review>> GetAllProductReviewsAsync(int productId)
    {
        var reviews = await _appDbContext.Reviews
            .Where(rew => rew.ProductId == productId)
            .OrderBy(date => date.DateCreated)
            .ToListAsync();
       
        return reviews;
    }

    public async Task<List<Review>> GetAllUserReviewsAsync(int userId)
    {
        var reviews = await _appDbContext.Reviews
            .Where(rew => rew.UserId == userId)
            .OrderByDescending(date => date.DateCreated)
            .ToListAsync();

        return reviews;
    }

    public async Task<Review> GetReviewsAsync(int rewiewId)
    {
        var review = await _appDbContext.Reviews.FindAsync(rewiewId);
        if(review == null)
        {
            throw new NotFoundExeption("Данный отзыв не найден");
        }

        return review;
    }

    public async Task UpdateRewiewAsync(Review newReview, int reviewId)
    {
        var review = await _appDbContext.Reviews.FindAsync(reviewId);
        if(review == null)
        {
            throw new NotFoundExeption("Не найден при обнавление отзыв");
        }

        review.ProductEvaluation = newReview.ProductEvaluation;
        review.Description = newReview.Description;
        review.DateCreated = newReview.DateCreated;
        review.Rating = newReview.Rating;

        await _appDbContext.SaveChangesAsync();
    }
}
