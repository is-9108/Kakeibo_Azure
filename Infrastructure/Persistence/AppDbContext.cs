using Kakeibo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kakeibo.Infrastructure.Persistence;

/// <summary>
/// SQL Server 用 EF Core DbContext
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
