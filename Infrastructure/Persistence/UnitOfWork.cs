using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public IRepository<User> Users { get; }
        public IRepository<Address> Addresses { get; }
        public IRepository<FashionProduct> FashionProducts { get; }
        public IRepository<ProductLike> ProductLikes { get; }
        public IRepository<CartItem> CartItems { get; }
        public IRepository<Purchase> Purchases { get; }
        public IRepository<Review> Reviews { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Users = new Repository<User>(_context);
            Addresses = new Repository<Address>(_context);
            FashionProducts = new Repository<FashionProduct>(_context);
            ProductLikes = new Repository<ProductLike>(_context);
            CartItems = new Repository<CartItem>(_context);
            Purchases = new Repository<Purchase>(_context);
            Reviews = new Repository<Review>(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
