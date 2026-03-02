using System.Collections.Concurrent;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;

namespace Kakeibo.Infrastructure.Repositories;

/// <summary>
/// サンプルエンティティ用のインメモリリポジトリ実装
/// （開発・テスト用。本番では DB 等の実装に差し替える）
/// </summary>
public class InMemorySampleRepository : IRepository<SampleEntity>
{
    private readonly ConcurrentDictionary<Guid, SampleEntity> _store = new();

    public Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<SampleEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = _store.Values.ToList().AsReadOnly();
        return Task.FromResult<IReadOnlyList<SampleEntity>>(list);
    }

    public Task AddAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        _store.TryAdd(entity.Id, entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        _store.AddOrUpdate(entity.Id, entity, (_, _) => entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(entity.Id, out _);
        return Task.CompletedTask;
    }
}
