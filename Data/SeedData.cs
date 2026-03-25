using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OskApi.Data;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Entities.User;

namespace OskApi.Data;

public class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MyDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // Veritabanı hazır olana kadar bekleme mantığı (Docker için)
        int retryCount = 0;
        while (retryCount < 5)
        {
            try
            {
                await context.Database.MigrateAsync();
                break;
            }
            catch (Exception)
            {
                retryCount++;
                await Task.Delay(5000);
                if (retryCount == 5) throw;
            }
        }

        // 1. Rol ve Admin Kullanıcı (Giriş yapabilmen için şart)
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new AppRole { Name = "Admin" });
        }

        if (await userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var user = new AppUser { UserName = "admin", Email = "admin@admin.com", EmailConfirmed = true };
            await userManager.CreateAsync(user, "Admin123*");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        // 2. Hastane Türleri (Frontend 'OH' beklediği için kodları güncelledik)
        if (!context.HealthFacilityTypes.Any())
        {
            context.HealthFacilityTypes.AddRange(
                new HealthFacilityType { Name = "Özel Hastane", Code = "OH" },
                new HealthFacilityType { Name = "Tıp Merkezi", Code = "TM" },
                new HealthFacilityType { Name = "Poliklinik", Code = "PK" },
                new HealthFacilityType { Name = "Muayenehane", Code = "MH" }
            );
            await context.SaveChangesAsync();
        }

        // 3. Hastaneler (Artık HFT-01 değil, OH arıyoruz)
        if (!context.HealthFacilities.Any())
        {
            var ozelHastane = await context.HealthFacilityTypes.FirstOrDefaultAsync(x => x.Code == "OH");

            if (ozelHastane != null)
            {
                context.HealthFacilities.Add(new HealthFacility
                {
                    Name = "Özel Acıbadem Hastanesi",
                    HealthFacilityTypeId = ozelHastane.Id,
                    Code = "HF-ACIBADEM"
                });
                await context.SaveChangesAsync();
            }
        }

        // 4. Personel Tipleri
        if (!context.PmTypes.Any())
        {
            context.PmTypes.AddRange(
                new PmType
                {
                    Name = "Kadrolu",
                    Code = "PMT-01",
                    Description = "Kadrolu personel",
                    Order = 1,
                    IsUsingStaff = true,
                    IsOnlyOneStatu = true,
                    StatusQuota = 1
                },
                new PmType
                {
                    Name = "16/3 Geçici Kadro",
                    Code = "PMT-02",
                    Description = "Geçici personel",
                    Order = 2,
                    IsUsingStaff = true,
                    IsOnlyOneStatu = true,
                    StatusQuota = 1
                }
            );
            await context.SaveChangesAsync();
        }
    }
}