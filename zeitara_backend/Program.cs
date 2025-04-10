using Infrastructure.Data;
using Infrastructure.DependencyInjection;

namespace zeitara_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastrcuture(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins); 

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
