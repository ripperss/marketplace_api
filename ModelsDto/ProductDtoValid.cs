using FluentValidation;
using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class ProductDtoValid : AbstractValidator<ProductDto>
{
    public ProductDtoValid()
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
