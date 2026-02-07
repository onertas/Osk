using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OskApi.Data;
using OskApi.Entities.User;
using OskApi.Services.HealthFacilities;
using OskApi.Services.Jwt;
using OskApi.Services.OpenAI;

namespace OskApi.ServiceRegisration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Db
           // services.AddDbContext<MyDbContext>(options =>
              // options.UseSqlServer(configuration.GetConnectionString("DefaultConnection1")));


            services.AddDbContext<MyDbContext>(options =>
        options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")))); ;
             
            // Identity
            services
                .AddIdentity<AppUser, IdentityRole<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();

            // Unit of Work
            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<MyDbContext>());

            // Custom Services
            services.AddScoped<IHealthFacilityService, HealthFacilityService>();
            services.AddScoped<IHealthFacilityTypeService, HealthFacilityTypeService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ChatService>();

            return services;
        }
    }
}
