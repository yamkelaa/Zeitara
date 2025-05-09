using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface ICartLogic
    {
        Task<InteractionResponseDto> AddToCartAsync(InteractionRequestDto request);
        Task<InteractionResponseDto> RemoveFromCartAsync(InteractionRequestDto request);
        Task<IEnumerable<FashionProductResponseDto>> GetCartItemsChunkAsync(int userId, int pageSize, int lastLoadedId = 0);
    }

}
