using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController(IPurchaseLogic _purchaseLogic) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> MakePurchase([FromBody] PurchaseRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0 || request.Quantity <= 0)
            return BadRequest("Invalid purchase data.");

        var result = await _purchaseLogic.MakePurchaseAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserPurchases(int userId, [FromQuery] int pageSize = 10, [FromQuery] int lastLoadedId = 0)
    {
        if (userId <= 0)
            return BadRequest("Invalid user ID.");

        if (pageSize <= 0)
            return BadRequest("Invalid page size.");

        var purchases = await _purchaseLogic.GetUserPurchasesChunkAsync(userId, pageSize, lastLoadedId);
        return Ok(purchases);
    }
}
