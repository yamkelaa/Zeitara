using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IReviewLogic
    {
        Task<InteractionResponseDto> AddReviewAsync(ReviewRequestDto request);
        Task<IEnumerable<FashionProductResponseDto>> GetProductReviewsWithStatusesAsync(int userId, int pageSize = 10, int lastLoadedId = 0);
        Task<InteractionResponseDto> DeleteReviewAsync(int reviewId, int userId);
    }
}
