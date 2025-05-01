using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces;

public interface IFashionProductLogic
{
    Task<FashionProductResponseDto?> GetFashionProductById(int id);
    Task<List<FashionProductResponseDto>> GetFashionProductsAsync(PaginationRequestDto pagination);
}
