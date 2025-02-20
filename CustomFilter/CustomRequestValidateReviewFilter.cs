using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomRequestValidateReviewFilter : IAsyncActionFilter
{
    private readonly IValidator<ReviewRequestDto> _validator;

    public CustomRequestValidateReviewFilter(IValidator<ReviewRequestDto> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var reviewsRequest = context.ActionArguments.Values.OfType<ReviewRequestDto>().FirstOrDefault();
        if (reviewsRequest != null)
        {
            var validate = await _validator.ValidateAsync(reviewsRequest);
            if (!validate.IsValid)
            {
                var errors = validate.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(r => r.Key
                    , r => r.Select(e => e.ErrorMessage)
                    .ToList());

                context.Result = new BadRequestObjectResult(new
                {
                    Error = errors
                });

                return;
            }
        }

        await next();
    }

}
