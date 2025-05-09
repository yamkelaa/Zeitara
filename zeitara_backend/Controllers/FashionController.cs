using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FashionProductController(IFashionProductLogic fashionProductLogic) : ControllerBase
{
    private readonly IFashionProductLogic _fashionProductLogic = fashionProductLogic;

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetProductDetails(int id, [FromQuery] int userId)
    {
        var product = await _fashionProductLogic.GetFashionProductById(id, userId);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsChunk(
        [FromQuery] int pageSize,
        [FromQuery] int userId,
        [FromQuery] int lastId = 0)
    {
        var products = await _fashionProductLogic.GetFashionProductsChunkAsync(pageSize, userId, lastId);
        return Ok(products);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFilteredProductsChunk(
        [FromQuery] int pageSize,
        [FromQuery] int userId,
        [FromQuery] int lastId = 0,
        [FromQuery] string? category = null,
        [FromQuery] string? color = null)
    {
        var products = await _fashionProductLogic.GetFilteredProductsChunkAsync(
            pageSize, userId, lastId, category, color);
        return Ok(products);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchProducts(
        [FromBody] FashionProductSearchRequestDto request,
        [FromQuery] int userId)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Query))
            return BadRequest("Search query is required.");

        var results = await _fashionProductLogic.SearchProductsAsync(request, userId);
        return Ok(results);
    }
}