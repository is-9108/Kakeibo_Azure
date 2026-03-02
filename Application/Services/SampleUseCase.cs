using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;

namespace Kakeibo.Application.Services;

/// <summary>
/// サンプルユースケースの実装
/// </summary>
public class SampleUseCase : ISampleUseCase
{
    private readonly IRepository<SampleEntity> _repository;

    public SampleUseCase(IRepository<SampleEntity> repository)
    {
        _repository = repository;
    }

    public async Task<SampleResponse> ExecuteAsync(SampleRequest request, CancellationToken cancellationToken = default)
    {
        var entity = SampleEntity.Create(request.Name, request.Amount);
        await _repository.AddAsync(entity, cancellationToken);

        return new SampleResponse(
            entity.Id,
            entity.Name,
            entity.Amount,
            entity.CreatedAt);
    }
}
