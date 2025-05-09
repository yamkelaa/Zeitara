using Application.Interfaces;
using Application.Logic;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DependencyInjection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastrcuture(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<IAppDBContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IFashionProductLogic, FashionProductLogic>();
            services.AddScoped<ILikeLogic, LikeLogic>();
            services.AddScoped<ICartLogic, CartLogic>();
            services.AddScoped<IReviewLogic, ReviewLogic>();
            services.AddScoped<IPurchaseLogic, PurchaseLogic>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
