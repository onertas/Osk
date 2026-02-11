using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Entities.User;
using System;

namespace OskApi.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MyDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            await context.Database.MigrateAsync();

            // Rol oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = "Admin"
                });
            }

            // Admin kullanıcı oluştur
            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Admin123*");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            // Sabit veri
            if (!context.HealthFacilityTypes.Any())
            {
                context.HealthFacilityTypes.AddRange(
                    new HealthFacilityType { Name = "Özel Hastane" },
                    new HealthFacilityType { Name = "Tıp Merkezi" },
                    new HealthFacilityType { Name = "Poliklinik" },
                    new HealthFacilityType { Name = "Muayenehane" }


                );

                await context.SaveChangesAsync();
            }

            if (!context.HealthFacilities.Any())
            {
                var ozelHastane = await context.HealthFacilityTypes
                .FirstAsync(x => x.Name == "Özel Hastane");

                context.HealthFacilities.AddRange(
                    new HealthFacility
                    {
                        Name = "Özel Acıbadem Hastanesi",
                        HealthFacilityTypeId = ozelHastane.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
