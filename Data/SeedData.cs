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

        // Veritabanı hazır olana kadar birkaç kez deneme yap (Retry Logic)
        int retryCount = 0;
        while (retryCount < 5)
        {
            try
            {
                await context.Database.MigrateAsync();
                break; // Başarılıysa döngüden çık
            }
            catch (Exception)
            {
                retryCount++;
                await Task.Delay(5000); // 5 saniye bekle ve tekrar dene
                if (retryCount == 5) throw; // 5 deneme de başarısızsa hata fırlat
            }
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