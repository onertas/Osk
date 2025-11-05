using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OskApi.Entities.HealthFacilities;

namespace OskApi.Data.Configurations;

    public class IcBedConfiguration : IEntityTypeConfiguration<IcBed>
    {
        public void Configure(EntityTypeBuilder<IcBed> builder)
        {
            builder.Property(p => p.IcBedRegLevel)
                .HasConversion(p => p.Value,p => IcBedRegLevel.FromValue(p));
            builder.Property(p => p.IcBedRegType)
               .HasConversion(p => p.Value, p => IcBedRegType.FromValue(p));
        }
    }

