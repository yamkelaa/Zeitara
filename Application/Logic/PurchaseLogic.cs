using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Logic
{
    public class PurchaseLogic : IPurchaseLogic
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InteractionResponseDto> PurchaseAllCartItemsAsync(int userId)
        {
            var cartItems = await _unitOfWork.CartItems
                .GetQueryable()
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return new InteractionResponseDto("Your cart is empty.");
            }

            var purchases = cartItems.Select(cartItem => new Purchase
            {
                UserId = userId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Status = "Completed",
                PurchaseDate = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Purchases.AddRangeAsync(purchases);

            _unitOfWork.CartItems.RemoveRange(cartItems);

            await _unitOfWork.SaveChangesAsync();

            return new InteractionResponseDto($"Successfully purchased {cartItems.Count} items.");
        }

        public async Task<IEnumerable<PurchaseResponseDto>> GetUserPurchasesChunkAsync(int userId, int pageSize, int lastLoadedId = 0)
        {
            var purchaseProductIdsQuery = _unitOfWork.Purchases
                .GetQueryable()
                .Where(p => p.UserId == userId)
                .Select(p => p.ProductId);

            var productsQuery = _unitOfWork.FashionProducts
                .GetQueryable()
                .Where(p => purchaseProductIdsQuery.Contains(p.Id));

            if (lastLoadedId > 0)
            {
                productsQuery = productsQuery.Where(p => p.Id > lastLoadedId);
            }

            var products = await productsQuery
                .OrderBy(p => p.Id)
                .Take(pageSize)
                .ToListAsync();

            var productIds = products.Select(p => p.Id).ToList();

            var cartItems = await _unitOfWork.CartItems
                .GetQueryable()
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .Select(c => c.ProductId)
                .ToListAsync();

            var likedItems = await _unitOfWork.ProductLikes
                .GetQueryable()
                .Where(l => l.UserId == userId && productIds.Contains(l.ProductId))
                .Select(l => l.ProductId)
                .ToListAsync();

            var purchasedItems = await _unitOfWork.Purchases
                .GetQueryable()
                .Where(p => p.UserId == userId && productIds.Contains(p.ProductId))
                .Select(p => p.ProductId)
                .Distinct()
                .ToListAsync();

            var ratings = await _unitOfWork.Reviews
                .GetQueryable()
                .Where(r => r.UserId == userId && productIds.Contains(r.ProductId))
                .Select(r => new { r.ProductId, r.Rating })
                .ToListAsync();

            var purchaseDtos = products.Select(p => new PurchaseResponseDto
            {
                PurchaseId = p.Id,
                ProductId = p.Id,
                Quantity = purchasedItems.Contains(p.Id) ? 1 : 0, 
                Status = purchasedItems.Contains(p.Id) ? "Completed" : "Pending",  
                PurchaseDate = DateTime.UtcNow,
                ProductDisplayName = p.ProductDisplayName,
                ImageUrl = $"images/{p.Id}.jpg",
                IsLiked = likedItems.Contains(p.Id),
                IsInCart = cartItems.Contains(p.Id),
                IsPurchased = purchasedItems.Contains(p.Id),
                UserRating = ratings.FirstOrDefault(r => r.ProductId == p.Id)?.Rating
            }).ToList();

            return purchaseDtos;
        }

    }
}
