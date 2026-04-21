using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace OskApi.Data;
public class MyDbContextFactory
        : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        // Migration çalışırken base path problemi yaşamamak için
        var basePath = Directory.GetCurrentDirectory();

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mysql =>
            {
                mysql.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            });

        return new MyDbContext(optionsBuilder.Options);
    }
}