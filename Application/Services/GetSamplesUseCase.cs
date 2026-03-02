using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;

namespace Kakeibo.Application.Services;

/// <summary>
/// サンプル一覧取得ユースケースの実装
/// </summary>
public class GetSamplesUseCase : IGetSamplesUseCase
{
    private readonly IRepository<SampleEntity> _repository;

    public GetSamplesUseCase(IRepository<SampleEntity> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SampleResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities
            .Select(e => new SampleResponse(e.Id, e.Name, e.Amount, e.CreatedAt))
            .ToList();
    }
}
