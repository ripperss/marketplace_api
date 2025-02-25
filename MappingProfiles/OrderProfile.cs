using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<OrderDto, Order>();
    }
}
