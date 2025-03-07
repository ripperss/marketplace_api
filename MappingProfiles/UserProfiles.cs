using AutoMapper;
using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.Mapping;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<UserDto, User>()
    .ForMember(user => user.Name, opt => opt.MapFrom(dto => dto.Name))
    .ForMember(user => user.Email, opt => opt.MapFrom(dto => dto.Email))
    .ForMember(user => user.HashPassword, opt => opt.MapFrom(dto => dto.HashPassword));

        CreateMap<User, UserDto>();

        CreateMap<User,UserResponseDto>();

        CreateMap<AdminDto, User>();
        CreateMap<User, AdminDto>();

        CreateMap<User, UserResponseDto>();

        CreateMap<User, UserLoginDto>();

        CreateMap<UserLoginDto, User>();
    }
}
