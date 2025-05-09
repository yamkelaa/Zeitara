using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IPurchaseLogic
    {
        Task<InteractionResponseDto> PurchaseAllCartItemsAsync(int userId);
        Task<IEnumerable<PurchaseResponseDto>> GetUserPurchasesChunkAsync(int userId, int pageSize, int lastLoadedId = 0);
    }
}
