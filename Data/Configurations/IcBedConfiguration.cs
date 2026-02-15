using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OskApi.Entities.Beds;
using OskApi.Entities.Personnel;


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

public class PersonnelBranchConfiguration : IEntityTypeConfiguration<PersonnelBranch>
{
    public void Configure(EntityTypeBuilder<PersonnelBranch> builder)
    {
        // Many-to-Many Composite Key Tanımlaması
        builder.HasKey(pb => new { pb.PersonnelId, pb.BranchId });

        builder.HasOne(pb => pb.Personnel)
            .WithMany(p => p.PersonnelBranches)
            .HasForeignKey(pb => pb.PersonnelId);

        builder.HasOne(pb => pb.Branch)
            .WithMany(b => b.PersonnelBranches)
            .HasForeignKey(pb => pb.BranchId);
    }
}

