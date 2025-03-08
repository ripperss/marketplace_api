using FluentValidation;

namespace marketplace_api.ModelsDto;

public class UserLoginDtoValidate : AbstractValidator<UserLoginDto> 
{
    public UserLoginDtoValidate()
    {
        RuleFor(user => user.HashPassword).NotEmpty();
        RuleFor(user => user.Email).NotEmpty();
    }
}
