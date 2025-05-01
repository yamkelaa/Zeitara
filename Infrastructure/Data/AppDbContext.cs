using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure.Data;

public class AppDbContext : DbContext, IAppDBContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<FashionProduct> FashionProducts => Set<FashionProduct>();

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
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("address");
            entity.HasKey(a => a.Address_Id);

            entity.HasOne<User>()
                  .WithOne()
                  .HasForeignKey<Address>(a => a.User_Id)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FashionProduct>(entity =>
        {
            entity.ToTable("fashionProducts");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Gender).IsRequired();
            entity.Property(f => f.MasterCategory).IsRequired();
            entity.Property(f => f.SubCategory).IsRequired();
            entity.Property(f => f.ArticleType).IsRequired();
            entity.Property(f => f.BaseColour).IsRequired();
            entity.Property(f => f.ProductDisplayName).IsRequired();

            entity.Property(f => f.Season).IsRequired(false);
            entity.Property(f => f.Year).IsRequired(false);
            entity.Property(f => f.Usage).IsRequired(false);
        });
    }
}
