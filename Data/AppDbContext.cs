using Microsoft.EntityFrameworkCore;
using TimescaleApi.Models;

namespace TimescaleApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Value> Values { get; set; }
    public DbSet<Result> Results { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Value>().ToTable("values");
        modelBuilder.Entity<Result>().ToTable("results");

        // Индекс для эффективности запросов (FileName DESC, Date DESC)
        modelBuilder.Entity<Value>()
            .HasIndex(v => new { v.FileName, v.Date })
            .IsDescending(true, true);  // FileName DESC, Date DESC

        // Primary key для Result
        modelBuilder.Entity<Result>()
            .HasKey(r => r.FileName);
    }
}