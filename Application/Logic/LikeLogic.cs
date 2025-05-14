using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Logic;

public class LikeLogic : ILikeLogic
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeLogic(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InteractionResponseDto> LikeProductAsync(InteractionRequestDto request)
    {
        var productExists = await _unitOfWork.FashionProducts
            .GetQueryable()
            .AnyAsync(p => p.Id == request.ProductId);

        if (!productExists)
            return new InteractionResponseDto("Product does not exist.");

        var exists = await _unitOfWork.ProductLikes
            .GetQueryable()
            .AnyAsync(l => l.UserId == request.UserId && l.ProductId == request.ProductId);

        if (exists)
            return new InteractionResponseDto("Product already liked.");

        await _unitOfWork.ProductLikes.AddAsync(new ProductLike
        {
            UserId = request.UserId,
            ProductId = request.ProductId
        });

        await _unitOfWork.SaveChangesAsync();
        return new InteractionResponseDto("Product liked successfully.");
    }

    public async Task<IEnumerable<FashionProductResponseDto>> GetLikedProductsChunkAsync(int userId, int pageSize, int lastLoadedId = 0)
    {
        var likedProductIdsQuery = _unitOfWork.ProductLikes
            .GetQueryable()
            .Where(l => l.UserId == userId)
            .Select(l => l.ProductId);

        var productsQuery = _unitOfWork.FashionProducts
            .GetQueryable()
            .Where(p => likedProductIdsQuery.Contains(p.Id));

        if (lastLoadedId > 0)
        {
            productsQuery = productsQuery.Where(p => p.Id > lastLoadedId);
        }

        var products = await productsQuery
            .OrderBy(p => p.Id)
            .Take(pageSize)
            .ToListAsync();

        return await MapProductsWithStatusAsync(products, userId, isLikedOverride: true);
    }

    public async Task<InteractionResponseDto> UnlikeProductAsync(InteractionRequestDto request)
    {
        var like = await _unitOfWork.ProductLikes
            .GetQueryable()
            .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.ProductId == request.ProductId);

        if (like == null)
            return new InteractionResponseDto("Product not found in likes.");

        _unitOfWork.ProductLikes.Remove(like);
        await _unitOfWork.SaveChangesAsync();

        return new InteractionResponseDto("Product unliked successfully.");
    }

    private async Task<List<FashionProductResponseDto>> MapProductsWithStatusAsync(
        List<FashionProduct> products,
        int userId,
        bool isLikedOverride = false)
    {
        if (products.Count == 0 || userId <= 0)
            return products.Select(p => new FashionProductResponseDto
            {
                Id = p.Id,
                Gender = p.Gender,
                MasterCategory = p.MasterCategory,
                SubCategory = p.SubCategory,
                ArticleType = p.ArticleType,
                BaseColour = p.BaseColour,
                Season = p.Season ?? "",
                Year = p.Year,
                Usage = p.Usage,
                ProductDisplayName = p.ProductDisplayName,
                ImageUrl = $"images/{p.Id}.jpg",
                IsLiked = isLikedOverride,
                IsInCart = false,
                IsPurchased = false,
                UserRating = null
            }).ToList();

        var productIds = products.Select(p => p.Id).ToList();

        var cart = await _unitOfWork.CartItems.GetQueryable()
            .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
            .Select(c => c.ProductId)
            .ToListAsync();

        var purchased = await _unitOfWork.Purchases.GetQueryable()
            .Where(p => p.UserId == userId && productIds.Contains(p.ProductId))
            .Select(p => p.ProductId)
            .Distinct()
            .ToListAsync();

        var ratings = await _unitOfWork.Reviews.GetQueryable()
            .Where(r => r.UserId == userId && productIds.Contains(r.ProductId))
            .Select(r => new { r.ProductId, r.Rating })
            .ToListAsync();

        // Only check likes if not overriding
        List<int> liked = isLikedOverride
            ? productIds // all are liked
            : await _unitOfWork.ProductLikes.GetQueryable()
                  .Where(l => l.UserId == userId && productIds.Contains(l.ProductId))
                  .Select(l => l.ProductId)
                  .ToListAsync();

        return products.Select(p => new FashionProductResponseDto
        {
            Id = p.Id,
            Gender = p.Gender,
            MasterCategory = p.MasterCategory,
            SubCategory = p.SubCategory,
            ArticleType = p.ArticleType,
            BaseColour = p.BaseColour,
            Season = p.Season ?? "",
            Year = p.Year,
            Usage = p.Usage,
            ProductDisplayName = p.ProductDisplayName,
            ImageUrl = $"https://res.cloudinary.com/dluhtovx4/image/upload/fashion-products/{p.Id}",
            IsLiked = liked.Contains(p.Id),
            IsInCart = cart.Contains(p.Id),
            IsPurchased = purchased.Contains(p.Id),
            UserRating = ratings.FirstOrDefault(r => r.ProductId == p.Id)?.Rating
        }).ToList();
    }
}
