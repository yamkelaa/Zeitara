using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure.Data;

public class AppDbContext : DbContext, IAppDBContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<FashionProduct> FashionProducts => Set<FashionProduct>();
    public DbSet<ProductLike> ProductLikes => Set<ProductLike>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<Review> Reviews => Set<Review>();

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

            entity.Property(u => u.User_Id).HasColumnName("user_id");
            entity.Property(u => u.Username).HasColumnName("username").IsRequired();
            entity.Property(u => u.User_Firstname).HasColumnName("user_firstname");
            entity.Property(u => u.User_Lastname).HasColumnName("user_lastname");
            entity.Property(u => u.User_Password).HasColumnName("user_password").IsRequired();
            entity.Property(u => u.Age).HasColumnName("age");
            entity.Property(u => u.Gender).HasColumnName("gender");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("address");
            entity.HasKey(a => a.Address_Id);

            entity.HasIndex(a => a.User_Id).IsUnique();

            entity.HasOne<User>()
                  .WithOne()
                  .HasForeignKey<Address>(a => a.User_Id)
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

        modelBuilder.Entity<ProductLike>(entity =>
        {
            entity.ToTable("ProductLikes");
            entity.HasKey(pl => pl.LikeId);

            entity.HasIndex(pl => new { pl.UserId, pl.ProductId }).IsUnique();

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(pl => pl.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<FashionProduct>()
                  .WithMany()
                  .HasForeignKey(pl => pl.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(ci => ci.CartItemId);

            entity.HasIndex(ci => new { ci.UserId, ci.ProductId }).IsUnique();

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(ci => ci.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<FashionProduct>()
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchases", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CHK_Purchases_Quantity", "[Quantity] > 0");
                tableBuilder.HasCheckConstraint("CHK_Purchases_TotalAmount", "[TotalAmount] > 0");
                tableBuilder.HasCheckConstraint("CHK_Purchases_Status",
                    "[Status] IN ('Pending', 'Completed', 'Shipped', 'Delivered', 'Cancelled')");
            });

            entity.HasKey(p => p.PurchaseId);
            entity.Property(p => p.Quantity).HasDefaultValue(1);
            entity.Property(p => p.PurchaseDate).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(p => p.Status).HasDefaultValue("Completed");

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<FashionProduct>()
                  .WithMany()
                  .HasForeignKey(p => p.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CHK_Reviews_Rating", "[Rating] BETWEEN 1 AND 5");
            });

            entity.HasKey(r => r.ReviewId);
            entity.Property(r => r.ReviewDate).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(r => r.Comment).HasMaxLength(1000);

            entity.HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<FashionProduct>()
                  .WithMany()
                  .HasForeignKey(r => r.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Purchase>()
                  .WithMany()
                  .HasForeignKey(r => r.PurchaseId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}

