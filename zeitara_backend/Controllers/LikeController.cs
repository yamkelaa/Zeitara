using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LikeController(ILikeLogic _likeLogic) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LikeProduct([FromBody] InteractionRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0 || request.Quantity > 1)
            return BadRequest("Invalid user or product ID.");

        var result = await _likeLogic.LikeProductAsync(request);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> UnlikeProduct([FromBody] InteractionRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0 || request.Quantity > 1)
            return BadRequest("Invalid user or product ID.");

        var result = await _likeLogic.UnlikeProductAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId}/chunk")]
    public async Task<IActionResult> GetLikedProductsChunk(int userId, [FromQuery] int pageSize, [FromQuery] int lastLoadedId = 0)
    {
        if (userId <= 0 || pageSize <= 0)
            return BadRequest("Invalid parameters.");

        var likedProducts = await _likeLogic.GetLikedProductsChunkAsync(userId, pageSize, lastLoadedId);
        return Ok(likedProducts);
    }
}
