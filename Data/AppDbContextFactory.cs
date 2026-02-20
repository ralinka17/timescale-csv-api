using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TimescaleApi.Data;          

namespace TimescaleApi.Data;       

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var connString = "Host=localhost;Port=5432;Database=timescale_db;Username=postgres;Password=1111";

        optionsBuilder.UseNpgsql(connString);


        return new AppDbContext(optionsBuilder.Options);
    }
}