using FluentValidation;
using marketplace_api.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace marketplace_api.CustomFilter;

public class CustomAdminValidteFilter : IAsyncActionFilter
{
    private readonly IValidator<AdminDto> _adminDtoValidate;

    public CustomAdminValidteFilter(IValidator<AdminDto> adminDtoValidate)
    {
        _adminDtoValidate = adminDtoValidate;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var adminDto = context.ActionArguments.Values.OfType<AdminDto>().FirstOrDefault();

        if (adminDto != null)
        {
            var validate = await _adminDtoValidate.ValidateAsync(adminDto);
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
