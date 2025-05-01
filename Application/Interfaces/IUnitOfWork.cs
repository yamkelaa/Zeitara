using Domain.Entities;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Address> Addresses { get; }
    IRepository<FashionProduct> FashionProducts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
