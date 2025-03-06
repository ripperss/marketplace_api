using FluentValidation;

namespace marketplace_api.ModelsDto;

public class AdminDtoValidate : AbstractValidator<AdminDto>
{
    public AdminDtoValidate()
    {
        RuleFor(admin => admin.Email).NotEmpty()
            .Must(email => email == "rippergods@gmail.com");

        RuleFor(admin => admin.Password).NotEmpty()
            .Must(pass => pass == "scammmm");
    }
}

