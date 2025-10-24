using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities;
using OskApi.Entities.User;

namespace OskApi.Data
{
    public class MyDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>,IUnitOfWork
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Identity tabloların isimlerini değiştirmek istersen burada yap
        }
    }
}
