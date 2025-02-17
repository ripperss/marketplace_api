using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<ProductDto, Product>();

        CreateMap<Product, ProductDto>()
            .ForMember(dto => dto.Id,
            opt => opt.MapFrom(product => product.Id)); 
    }
}
