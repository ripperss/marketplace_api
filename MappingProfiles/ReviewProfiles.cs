using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.MappingProfiles;

public class ReviewProfiles : Profile
{
    public ReviewProfiles()  
    { 
        CreateMap<Review, ReviewResponseDto>();
        CreateMap<ReviewResponseDto, Review>();

        CreateMap<Review, ReviewRequestDto>();
        CreateMap<ReviewRequestDto, Review>();
    }
}
