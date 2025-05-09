using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Address> Addresses { get; }
        IRepository<FashionProduct> FashionProducts { get; }
        IRepository<ProductLike> ProductLikes { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Purchase> Purchases { get; }
        IRepository<Review> Reviews { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
