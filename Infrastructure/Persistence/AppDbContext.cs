using Kakeibo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kakeibo.Infrastructure.Persistence;

/// <summary>
/// SQL Server 用 EF Core DbContext
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<TransactionEntity> transactions => Set<TransactionEntity>();
    public DbSet<MonthlySummaryEntity> MonthlySummaries => Set<MonthlySummaryEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
