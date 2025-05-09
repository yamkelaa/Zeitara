using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IFashionProductLogic
    {
        Task<FashionProductResponseDto?> GetFashionProductById(int productId, int userId);
        Task<IEnumerable<FashionProductResponseDto>> GetFashionProductsChunkAsync(
            int pageSize, int userId, int lastLoadedId = 0);
        Task<IEnumerable<FashionProductResponseDto>> GetFilteredProductsChunkAsync(
            int pageSize, int userId, int lastLoadedId = 0,
            string? categoryFilter = null, string? colorFilter = null);
        Task<IEnumerable<FashionProductResponseDto>> SearchProductsAsync(
            FashionProductSearchRequestDto searchRequest, int userId);
    }
}