using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OskApi.Entities.HealthFacilities;

namespace OskApi.Data.Configurations;

public class IcBedNameConfiguration : IEntityTypeConfiguration<IcBedName>
{
    public void Configure(EntityTypeBuilder<IcBedName> builder)
    {
        builder.Property(p => p.IcBedType)
            .HasConversion(p => p.Value, p => IcBedType.FromValue(p));
       
    }
}

