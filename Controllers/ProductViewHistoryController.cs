using marketplace_api.Services.ProductViewHistoryService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class ProductViewHistoryController : ControllerBase
{
    private readonly IProductViewHistoryService _productViewHistoryService;

    public ProductViewHistoryController(IProductViewHistoryService productViewHistoryService)
    {
        _productViewHistoryService = productViewHistoryService;
    }
}
