using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<User> Users { get; }
    public IRepository<Address> Addresses { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Users = new Repository<User>(_context);
        Addresses = new Repository<Address>(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
