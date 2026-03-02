using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;
using Kakeibo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kakeibo.Infrastructure.Repositories;

/// <summary>
/// SampleEntity 用の SQL Server リポジトリ実装
/// </summary>
public class SqlServerSampleRepository : IRepository<SampleEntity>
{
    private readonly AppDbContext _db;

    public SqlServerSampleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.SampleEntities.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<SampleEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.SampleEntities.OrderBy(e => e.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        await _db.SampleEntities.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        _db.SampleEntities.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        _db.SampleEntities.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
