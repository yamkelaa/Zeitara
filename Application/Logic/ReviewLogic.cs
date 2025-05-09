using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Logic
{
    public class ReviewLogic : IReviewLogic
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InteractionResponseDto> AddReviewAsync(ReviewRequestDto request)
        {
            var productExists = await _unitOfWork.FashionProducts
                .GetQueryable()
                .AnyAsync(p => p.Id == request.ProductId);

            if (!productExists)
                return new InteractionResponseDto("Product does not exist.");

            var alreadyReviewed = await _unitOfWork.Reviews
                .GetQueryable()
                .AnyAsync(r => r.UserId == request.UserId && r.ProductId == request.ProductId);

            if (alreadyReviewed)
                return new InteractionResponseDto("You have already reviewed this product.");

            var review = new Review
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment,
                PurchaseId = request.PurchaseId,
                ReviewDate = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return new InteractionResponseDto("Review added successfully.");
        }

        public async Task<IEnumerable<FashionProductResponseDto>> GetProductReviewsWithStatusesAsync(int userId, int pageSize = 10, int lastLoadedId = 0)
        {
            var reviewsQuery = _unitOfWork.Reviews
                .GetQueryable()
                .Where(r => r.UserId == userId);

            // Apply pagination logic for reviews
            if (lastLoadedId > 0)
            {
                reviewsQuery = reviewsQuery.Where(r => r.ReviewId > lastLoadedId);
            }

            var reviews = await reviewsQuery
                .OrderByDescending(r => r.ReviewDate)
                .Take(pageSize)
                .ToListAsync();

            var productIds = reviews.Select(r => r.ProductId).Distinct().ToList();

            // Fetch product status: liked, in cart, purchased
            var cartProductIds = await _unitOfWork.CartItems.GetQueryable()
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .Select(c => c.ProductId)
                .ToListAsync();

            var likedProductIds = await _unitOfWork.ProductLikes.GetQueryable()
                .Where(l => l.UserId == userId && productIds.Contains(l.ProductId))
                .Select(l => l.ProductId)
                .ToListAsync();

            var purchasedProductIds = await _unitOfWork.Purchases.GetQueryable()
                .Where(p => p.UserId == userId && productIds.Contains(p.ProductId))
                .Select(p => p.ProductId)
                .ToListAsync();

            var ratings = await _unitOfWork.Reviews.GetQueryable()
                .Where(r => r.UserId == userId && productIds.Contains(r.ProductId))
                .Select(r => new { r.ProductId, r.Rating })
                .ToListAsync();

            // Map all the products with their statuses
            var products = await _unitOfWork.FashionProducts
                .GetQueryable()
                .Where(p => productIds.Contains(p.Id))
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
                IsLiked = likedProductIds.Contains(p.Id),
                IsInCart = cartProductIds.Contains(p.Id),
                IsPurchased = purchasedProductIds.Contains(p.Id),
                UserRating = ratings.FirstOrDefault(r => r.ProductId == p.Id)?.Rating,
            }).ToList();
        }



        public async Task<InteractionResponseDto> DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _unitOfWork.Reviews
                .GetQueryable()
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

            if (review == null)
                return new InteractionResponseDto("Review not found or not owned by user.");

            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.SaveChangesAsync();

            return new InteractionResponseDto("Review deleted successfully.");
        }

    }
}
