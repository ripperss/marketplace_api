using FluentValidation;

namespace marketplace_api.ModelsDto;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(product => product.Name).NotEmpty()
            .Length(3, 20);
        RuleFor(product => product.Description).NotEmpty()
            .Length(20, 600);
        RuleFor(product => product.Price).NotEmpty()
            .LessThanOrEqualTo(10000);
        RuleFor(product => product.Category).NotEmpty();
    }
}
