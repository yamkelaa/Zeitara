using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDBContext
{
    DbSet<User> Users { get; }
    DbSet<Address> Addresses { get; }
}
