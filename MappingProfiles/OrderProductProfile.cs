using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.MappingProfiles;

public class OrderProductProfile : Profile
{
    public  OrderProductProfile()
    {
        CreateMap<OrderProductDto, OrderProduct>();

        CreateMap<OrderProduct, OrderProductDto>();
    }
}
