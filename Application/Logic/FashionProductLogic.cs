using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Logic
{
    public class FashionProductLogic(IUnitOfWork _unitOfWork) : IFashionProductLogic
    {
        public async Task<FashionProductResponseDto?> GetFashionProductById(int productId)
        {
            var product = await _unitOfWork.FashionProducts.FindAsync(p => p.Id == productId);
            var matchedProduct = product.FirstOrDefault();
            if (matchedProduct == null) return null;

            return new FashionProductResponseDto
            {
                Id = matchedProduct.Id,
                Gender = matchedProduct.Gender,
                MasterCategory = matchedProduct.MasterCategory,
                SubCategory = matchedProduct.SubCategory,
                ArticleType = matchedProduct.ArticleType,
                BaseColour = matchedProduct.BaseColour,
                Season = matchedProduct.Season ?? "",
                Year = matchedProduct.Year,
                Usage = matchedProduct.Usage,
                ProductDisplayName = matchedProduct.ProductDisplayName,
                ImageUrl = $"https://res.cloudinary.com/dluhtovx4/image/upload/fashion-products/{matchedProduct.Id}"
            };
        }

        public async Task<List<FashionProductResponseDto>> GetFashionProductsAsync(PaginationRequestDto pagination)
        {
            var products = await _unitOfWork.FashionProducts.GetAllAsync();

            var pagedProducts = products
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(product => new FashionProductResponseDto
                {
                    Id = product.Id,
                    Gender = product.Gender,
                    MasterCategory = product.MasterCategory,
                    SubCategory = product.SubCategory,
                    ArticleType = product.ArticleType,
                    BaseColour = product.BaseColour,
                    Season = product.Season ?? "",
                    Year = product.Year,
                    Usage = product.Usage,
                    ProductDisplayName = product.ProductDisplayName,
                    ImageUrl = $"https://res.cloudinary.com/dluhtovx4/image/upload/fashion-products/{product.Id}"
                })
                .ToList();

            return pagedProducts;
        }
    }
}
