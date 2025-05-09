using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Logic;

public class CartLogic : ICartLogic
{
    private readonly IUnitOfWork _unitOfWork;

    public CartLogic(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InteractionResponseDto> AddToCartAsync(InteractionRequestDto request)
    {
        var productExists = await _unitOfWork.FashionProducts
       .GetQueryable()
       .AnyAsync(p => p.Id == request.ProductId);

        if (!productExists)
            return new InteractionResponseDto("Product does not exist.");

        var exists = await _unitOfWork.CartItems
            .GetQueryable()
            .AnyAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

        if (exists)
            return new InteractionResponseDto("Product already in cart.");

        await _unitOfWork.CartItems.AddAsync(new CartItem
        {
            UserId = request.UserId,
            ProductId = request.ProductId
        });

        await _unitOfWork.SaveChangesAsync();
        return new InteractionResponseDto("Product added to cart successfully.");
    }

    public async Task<IEnumerable<FashionProductResponseDto>> GetCartItemsChunkAsync(int userId, int pageSize, int lastLoadedId = 0)
    {
        var cartProductIdsQuery = _unitOfWork.CartItems
            .GetQueryable()
            .Where(c => c.UserId == userId)
            .Select(c => c.ProductId);

        var productsQuery = _unitOfWork.FashionProducts
            .GetQueryable()
            .Where(p => cartProductIdsQuery.Contains(p.Id));

        if (lastLoadedId > 0)
        {
            productsQuery = productsQuery.Where(p => p.Id > lastLoadedId);
        }

        var products = await productsQuery
            .OrderBy(p => p.Id)
            .Take(pageSize)
            .ToListAsync();

        var productIds = products.Select(p => p.Id).ToList();

        var liked = await _unitOfWork.ProductLikes.GetQueryable()
            .Where(l => l.UserId == userId && productIds.Contains(l.ProductId))
            .Select(l => l.ProductId)
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
            IsInCart = true,
            IsLiked = liked.Contains(p.Id),
            IsPurchased = purchased.Contains(p.Id),
            UserRating = ratings.FirstOrDefault(r => r.ProductId == p.Id)?.Rating
        }).ToList();
    }



    public async Task<InteractionResponseDto> RemoveFromCartAsync(InteractionRequestDto request)
    {
        var cartItem = await _unitOfWork.CartItems
            .GetQueryable()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

        if (cartItem == null)
            return new InteractionResponseDto("Product not found in cart.");

        _unitOfWork.CartItems.Remove(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return new InteractionResponseDto("Product removed from cart successfully.");
    }


}
