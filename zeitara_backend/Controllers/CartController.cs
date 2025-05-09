using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CartController(ICartLogic _cartLogic) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] InteractionRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0)
            return BadRequest("Invalid user or product ID.");

        var result = await _cartLogic.AddToCartAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId}/chunk")]
    public async Task<IActionResult> GetCartItemsChunk(int userId, [FromQuery] int pageSize, [FromQuery] int lastLoadedId = 0)
    {
        if (userId <= 0 || pageSize <= 0)
            return BadRequest("Invalid parameters.");

        var cartItems = await _cartLogic.GetCartItemsChunkAsync(userId, pageSize, lastLoadedId);
        return Ok(cartItems);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveFromCart([FromBody] InteractionRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0)
            return BadRequest("Invalid user or product ID.");

        var result = await _cartLogic.RemoveFromCartAsync(request);

        if (result.Message.Contains("not found"))
            return NotFound(result);

        return Ok(result);
    }
}
