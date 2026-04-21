using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OskApi.Data;
using OskApi.Entities.User;
using OskApi.Services.OpenAI;
using System.Reflection;

namespace OskApi.ServiceRegisration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Db
            services.AddDbContext<MyDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 0)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null)));

            // Identity
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

            // Unit Of Work
            services.AddScoped<IUnitOfWork>(srv =>
                srv.GetRequiredService<MyDbContext>());

            // 🔵 Service otomatik register
            RegisterServices(services);

            // Interface olmayan servisler
            services.AddScoped<ChatService>();

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"));

            foreach (var implementation in serviceTypes)
            {
                var interfaceType = implementation.GetInterface($"I{implementation.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementation);
                }
            }
        }
    }
}