using AutoMapper;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Repository.Rewiew;
using marketplace_api.Services.ProductService;
using marketplace_api.Services.UserService;

namespace marketplace_api.Services.ReviewService;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IUserService _userService; 

    public ReviewService(
        IReviewRepository reviewRepository
        , IMapper mapper
        , IProductService productService
        , IUserService userService)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _productService = productService;
        _userService = userService;
    }

    public async Task CreateRewiewAsync(ReviewRequestDto reviewDto)
    {
        if (reviewDto == null)
        {
            throw new ArgumentNullException("отзыв не может быть равен null");
        }

        var review = _mapper.Map<Review>(reviewDto);

        var user = await _userService.GetByIndexUserAsync(reviewDto.UserId);
        review.User = user;

        await _reviewRepository.CreateRewiewAsync(review);

    }

    public async Task DeleteRewiewAsync(int rewiewId)
    {
        if(rewiewId <= 0)
        {
            throw new NotFoundExeption("отзыв по данному индексу не найден");
        }

        await _reviewRepository.DeleteRewiewAsync(rewiewId);
    }

    public async Task<List<ReviewResponseDto>> GetAllProductReviewsAsync(int productId)
    {
        await _productService.GetProductAsync(productId);

        var reviews = await _reviewRepository.GetAllProductReviewsAsync(productId);
        var reviewsDto = _mapper.Map<List<ReviewResponseDto>>(reviews);

        return reviewsDto;
    }

    public async Task<List<ReviewResponseDto>> GetAllUserReviewsAsync(int userId)
    {
        var reviews = await _reviewRepository.GetAllUserReviewsAsync(userId);

        var reviewsDto = _mapper.Map<List<ReviewResponseDto>>(reviews);

        return reviewsDto;
    }

    public async Task<ReviewResponseDto> GetReviewsAsync(int rewiewid)
    {
        if(rewiewid <= 0)
        {
            throw new NotFoundExeption("с данным id  пользователь не найден");
        }

        var review = await _reviewRepository.GetReviewsAsync(rewiewid);
        var reviewdto = _mapper.Map<ReviewResponseDto>(review);

        return reviewdto;
    }

    public async Task UpdateRewiewAsync(ReviewRequestDto newReviewDto, int reviewId)
    {
        if (reviewId <= 0)
        {
            throw new NotFoundExeption("с данным id  пользователь не найден");
        }
        var newRview = _mapper.Map<Review>(newReviewDto);
        var user = await _userService.GetByIndexUserAsync(newReviewDto.UserId);
        newRview.User = user;

        await _reviewRepository.UpdateRewiewAsync(newRview, reviewId);
    }
}
