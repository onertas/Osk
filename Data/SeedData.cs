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

        await context.Database.MigrateAsync();

        // Rol ve Admin Kullanıcı kısımları doğru (Code alanı AppUser'da yoksa sorun çıkarmaz)

        // Sabit veri: HealthFacilityTypes
        if (!context.HealthFacilityTypes.Any())
        {
            context.HealthFacilityTypes.AddRange(
                new HealthFacilityType { Name = "Özel Hastane", Code = "HFT-01" },
                new HealthFacilityType { Name = "Tıp Merkezi", Code = "HFT-02" },
                new HealthFacilityType { Name = "Poliklinik", Code = "HFT-03" },
                new HealthFacilityType { Name = "Muayenehane", Code = "HFT-04" }
            );
            await context.SaveChangesAsync();
        }

        // HealthFacilities
        if (!context.HealthFacilities.Any())
        {
            // FirstAsync yerine kod ile bulmak daha güvenlidir
            var ozelHastane = await context.HealthFacilityTypes.FirstOrDefaultAsync(x => x.Code == "HFT-01");

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

        // PmTypes
        if (!context.PmTypes.Any())
        {
            context.PmTypes.AddRange(
                new PmType
                {
                    Name = "Kadrolu",
                    Code = "PMT-01", // Code alanını ekledik
                    Description = "Kadrolu personel",
                    Order = 1,
                    IsUsingStaff = true,
                    IsOnlyOneStatu = true,
                    StatusQuota = 1
                },
                new PmType
                {
                    Name = "16/3 Geçici Kadro",
                    Code = "PMT-02", // Code alanını ekledik
                    Description = "Geçici personel",
                    Order = 2, // Sıralamayı değiştirdik
                    IsUsingStaff = true,
                    IsOnlyOneStatu = true,
                    StatusQuota = 1
                }
            );
            await context.SaveChangesAsync();
        }
    }
}