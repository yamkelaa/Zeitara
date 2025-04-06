using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure.Data;

public class AppDbContext : DbContext, IAppDBContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(u => u.User_Id);
            entity.HasOne(u => u.Address)
                  .WithOne(a => a.User)
                  .HasForeignKey<Address>(a => a.User_Id)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => a.Address_Id); 
            entity.ToTable("address");
        });
    }
}
