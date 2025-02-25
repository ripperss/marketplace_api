using marketplace_api.Services.ReviewService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using marketplace_api.CustomFilter;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.Controllers;

[ServiceFilter(typeof(CustomResponseValidateReviewFilter))]
[ServiceFilter(typeof(CustomRequestValidateReviewFilter))]
[ApiController]
[Route("{controller}")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;
    
    public ReviewController(
        IReviewService reviewService
        , ILogger<ReviewController> logger)
    {
        _reviewService = reviewService; 
        _logger = logger;
    }
    
    [Authorize(Roles = "Admin, User, Seller")]
    [HttpPost]
    [Route("create_review")]
    public async Task<IActionResult> CreateReviewAsync(ReviewRequestDto requestDto)
    {
        _logger.LogInformation("Starting review update for review ");

        await _reviewService.CreateRewiewAsync(requestDto);

        return StatusCode(201);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    [Route(("update_review/{reviewId}"))]
    public async Task<IActionResult> UpdateReviewAsync(ReviewRequestDto reviewRequestDto, int reviewId)
    {
        _logger.LogInformation("Starting review deletion for review ");

        await _reviewService.UpdateRewiewAsync(reviewRequestDto, reviewId);

        return StatusCode(201);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("delete_review/{reviewId}")]
    public async Task<IActionResult> DeleteREviewAsync(int reviewId)
    {
        _logger.LogInformation("Retrieving reviews for product");

        await _reviewService.DeleteRewiewAsync(reviewId);

        return NoContent();
    }

    [HttpGet]
    [Route("reviews_product/{productId}")]
    public async Task<IActionResult> GetReviewsProductAsync(int productId)
    {
        _logger.LogInformation("Retrieving reviews for user");

        var reviews = await _reviewService.GetAllProductReviewsAsync(productId);

        return Ok((reviews));
    }

    [HttpGet]
    [Route("reviews_user/{userId}")]
    public async Task<IActionResult> GetReviewsUserAsync(int userId)
    {
        _logger.LogInformation("Retrieving review ");

        var reviews = await _reviewService.GetAllUserReviewsAsync(userId);

        return Ok(reviews);
    }

    [HttpGet]
    [Route("review/{reviewId}")]
    public async Task<IActionResult> GetReviewAsync(int reviewId)
    {
        var review = await _reviewService.GetReviewsAsync(reviewId);

        return Ok(review);
    }
}
