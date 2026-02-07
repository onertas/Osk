using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities;
using OskApi.Entities.Beds;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Entities.User;
using System;
using System.Reflection.Emit;
namespace OskApi.Data
{
    public class MyDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; }
        public DbSet<HealthFacility> HealthFacilities { get; set; }
        public DbSet<IcBedName> IcBedNames { get; set; } = null!;
        public DbSet<IcBed> IcBeds { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Branch> Branches { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(Guid))
                    {
                        property.SetColumnType("char(36)");
                        property.SetMaxLength(36);
                    }
                }
            }
            base.OnModelCreating(builder);
            // Identity tabloların isimlerini değiştirmek istersen burada yap
        }
    }
}
