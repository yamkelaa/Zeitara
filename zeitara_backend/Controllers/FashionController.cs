using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FashionProductController(IFashionProductLogic _fashionProductLogic) : ControllerBase
{
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetFashionProductById(int id)
    {
        var product = await _fashionProductLogic.GetFashionProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(new ApiResponse<FashionProductResponseDto>(product));
    }

    [HttpPost]
    [Route("Paged")]
    public async Task<IActionResult> GetFashionProductsPaged([FromBody] PaginationRequestDto pagination)
    {
        var products = await _fashionProductLogic.GetFashionProductsAsync(pagination);
        return Ok(new ApiResponse<List<FashionProductResponseDto>>(products));
    }
}

