using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IPurchaseLogic
    {
        Task<InteractionResponseDto> MakePurchaseAsync(PurchaseRequestDto request);
        Task<IEnumerable<PurchaseResponseDto>> GetUserPurchasesChunkAsync(int userId, int pageSize, int lastLoadedId = 0);
    }
}
