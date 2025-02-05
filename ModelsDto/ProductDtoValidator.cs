using FluentValidation;

namespace marketplace_api.ModelsDto;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(product => product.Name).NotEmpty();
        RuleFor(product => product.Description).NotEmpty();
        RuleFor(product => product.Price).NotEmpty();
        RuleFor(product => product.Category).NotEmpty();
    }
}
    