using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OskApi.Entities.HealthFacilities;

namespace OskApi.Data.Configurations
{
    public class HealthFacilityConfiguration : IEntityTypeConfiguration<HealthFacility>
    {
        public void Configure(EntityTypeBuilder<HealthFacility> builder)
        {
            builder.Property(p => p.HfStatus)
                .HasConversion(
                    p => p.Value,
                    p => HfStatus.FromValue(p)
                );
        }
    }
}
