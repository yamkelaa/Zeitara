using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface ILikeLogic
    {
        Task<InteractionResponseDto> LikeProductAsync(InteractionRequestDto request);
        Task<InteractionResponseDto> UnlikeProductAsync(InteractionRequestDto request);
        Task<IEnumerable<FashionProductResponseDto>> GetLikedProductsChunkAsync(int userId, int pageSize, int lastLoadedId = 0);
    }
}
