using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Logic
{
    public class FashionProductLogic : IFashionProductLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string AssetsBasePath = "images/";

        public FashionProductLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<FashionProductResponseDto?> GetFashionProductById(int productId, int userId)
        {
            if (productId <= 0)
            {
                return null;
            }

            var product = await _unitOfWork.FashionProducts.GetByIdAsync(productId);
            if (product == null) return null;

            var interactionStatus = userId > 0
                ? await GetFullInteractionStatus(productId, userId)
                : (false, false, false, null);

            return MapToResponseDto(product, interactionStatus);
        }

        public async Task<IEnumerable<FashionProductResponseDto>> GetFashionProductsChunkAsync(
            int pageSize, int userId, int lastLoadedId = 0)
        {
            if (pageSize <= 0)
            {
                return Enumerable.Empty<FashionProductResponseDto>();
            }

            var products = await _unitOfWork.FashionProducts.GetQueryable()
                .Where(p => p.Id > lastLoadedId)
                .OrderBy(p => p.Id)
                .Take(pageSize)
                .ToListAsync();

            return await MapProductsWithStatus(products, userId);
        }

        public async Task<IEnumerable<FashionProductResponseDto>> GetFilteredProductsChunkAsync(
            int pageSize, int userId, int lastLoadedId = 0, string? categoryFilter = null, string? colorFilter = null)
        {
            if (pageSize <= 0)
            {
                return Enumerable.Empty<FashionProductResponseDto>();
            }

            var query = _unitOfWork.FashionProducts.GetQueryable()
                .Where(p => p.Id > lastLoadedId);

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                query = query.Where(p => p.MasterCategory == categoryFilter || p.SubCategory == categoryFilter);
            }

            if (!string.IsNullOrEmpty(colorFilter))
            {
                query = query.Where(p => p.BaseColour == colorFilter);
            }

            var products = await query
                .OrderBy(p => p.Id)
                .Take(pageSize)
                .ToListAsync();

            return await MapProductsWithStatus(products, userId);
        }

        public async Task<IEnumerable<FashionProductResponseDto>> SearchProductsAsync(
            FashionProductSearchRequestDto searchRequest, int userId)
        {
            if (searchRequest == null || string.IsNullOrWhiteSpace(searchRequest.Query))
            {
                return Enumerable.Empty<FashionProductResponseDto>();
            }

            var terms = searchRequest.Query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.ToLower())
                .ToList();

            var query = _unitOfWork.FashionProducts.GetQueryable()
                .Where(p => terms.Any(term =>
                    p.ProductDisplayName.ToLower().Contains(term) ||
                    p.MasterCategory.ToLower().Contains(term) ||
                    p.SubCategory.ToLower().Contains(term) ||
                    p.ArticleType.ToLower().Contains(term) ||
                    p.BaseColour.ToLower().Contains(term) ||
                    p.Gender.ToLower().Contains(term) ||
                    (p.Season != null && p.Season.ToLower().Contains(term)) ||
                    (p.Usage != null && p.Usage.ToLower().Contains(term))
                ))
                .Take(5);

            var products = await query.ToListAsync();
            return await MapProductsWithStatus(products, userId);
        }

        private async Task<(bool IsLiked, bool IsInCart, bool IsPurchased, int? UserRating)> GetFullInteractionStatus(
            int productId, int userId)
        {
            var likeTask =await _unitOfWork.ProductLikes.GetQueryable()
                .AnyAsync(l => l.ProductId == productId && l.UserId == userId);

            var cartTask = await _unitOfWork.CartItems.GetQueryable()
                .AnyAsync(c => c.ProductId == productId && c.UserId == userId);

            var purchaseTask = await _unitOfWork.Purchases.GetQueryable()
                .AnyAsync(p => p.ProductId == productId && p.UserId == userId);

            var reviewTask = await _unitOfWork.Reviews.GetQueryable()
                .Where(r => r.ProductId == productId && r.UserId == userId)
                .Select(r => (int?)r.Rating)
                .FirstOrDefaultAsync();

            return (likeTask, cartTask , purchaseTask, reviewTask);
        }

        private async Task<List<FashionProductResponseDto>> MapProductsWithStatus(
            List<FashionProduct> products, int userId)
        {
            if (products.Count == 0)
            {
                return new List<FashionProductResponseDto>();
            }

            if (userId <= 0)
            {
                return products.Select(p => MapToResponseDto(p, (false, false, false, null))).ToList();
            }

            var productIds = products.Select(p => p.Id).ToList();

            var likedProducts = await _unitOfWork.ProductLikes.GetQueryable()
                .Where(l => l.UserId == userId && productIds.Contains(l.ProductId))
                .Select(l => l.ProductId)
                .ToListAsync();

            var cartProducts = await _unitOfWork.CartItems.GetQueryable()
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .Select(c => c.ProductId)
                .ToListAsync();

            var purchasedProducts = await _unitOfWork.Purchases.GetQueryable()
                .Where(p => p.UserId == userId && productIds.Contains(p.ProductId))
                .Select(p => p.ProductId)
                .Distinct()
                .ToListAsync();

            var userRatings = await _unitOfWork.Reviews.GetQueryable()
                .Where(r => r.UserId == userId && productIds.Contains(r.ProductId))
                .Select(r => new { r.ProductId, r.Rating })
                .ToDictionaryAsync(r => r.ProductId, r => (int?)r.Rating);

            return products.Select(p =>
            {
                var interactionStatus = (
                    likedProducts.Contains(p.Id),
                    cartProducts.Contains(p.Id),
                    purchasedProducts.Contains(p.Id),
                    userRatings.GetValueOrDefault(p.Id)
                );
                return MapToResponseDto(p, interactionStatus);
            }).ToList();
        }

        private FashionProductResponseDto MapToResponseDto(
            FashionProduct product,
            (bool IsLiked, bool IsInCart, bool IsPurchased, int? UserRating) interactionStatus)
        {
            return new FashionProductResponseDto
            {
                Id = product.Id,
                Gender = product.Gender,
                MasterCategory = product.MasterCategory,
                SubCategory = product.SubCategory,
                ArticleType = product.ArticleType,
                BaseColour = product.BaseColour,
                Season = product.Season ?? string.Empty,
                Year = product.Year,
                Usage = product.Usage ?? string.Empty,
                ProductDisplayName = product.ProductDisplayName,
                ImageUrl = $"{AssetsBasePath}{product.Id}",
                IsLiked = interactionStatus.IsLiked,
                IsInCart = interactionStatus.IsInCart,
                IsPurchased = interactionStatus.IsPurchased,
                UserRating = interactionStatus.UserRating
            };
        }
    }
}