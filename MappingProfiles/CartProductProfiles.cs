using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.MappingProfiles;

public class CartProductProfiles : Profile
{
    public CartProductProfiles()
    {
        CreateMap<CartProduct,CartProductDto>();

        CreateMap<CartProductDto,CartProduct > ();
    }
}
