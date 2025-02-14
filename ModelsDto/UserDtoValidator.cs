using FluentValidation;


namespace marketplace_api.ModelsDto;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(user => user.Name).NotEmpty();
        RuleFor(user => user.Email).NotEmpty();
        RuleFor(user => user.Email).Length(10, 30);
        RuleFor(user => user.Name).Length(4, 10);
        RuleFor(user => user.HashPassword).Length(10, 20);
    }
}
