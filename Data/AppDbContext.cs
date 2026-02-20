using Microsoft.EntityFrameworkCore;
using TimescaleApi.Models;

namespace TimescaleApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<DataPoint> DataPoints { get; set; }  
    public DbSet<Result> Results { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DataPoint>()                          
            .HasIndex(v => new { v.FileName, v.Date })
            .IsDescending(); // FileName DESC + Date DESC.IsDescending(false, true)

        modelBuilder.Entity<Result>()
            .HasKey(r => r.FileName);
    }
}