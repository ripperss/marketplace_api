using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomResponseValidateReviewFilter : IAsyncActionFilter 
{
    private readonly IValidator<ReviewResponseDto> _responseValidator;

    public CustomResponseValidateReviewFilter(IValidator<ReviewResponseDto> responseValidator)
    {
        _responseValidator = responseValidator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var reviewResponse = context.ActionArguments.Values.OfType<ReviewResponseDto>().FirstOrDefault();
        if (reviewResponse != null)
        {
            var validate = await _responseValidator.ValidateAsync(reviewResponse);
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
