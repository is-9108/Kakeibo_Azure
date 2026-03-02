using Kakeibo.Application.DTOs;

namespace Kakeibo.Application.Interfaces;

/// <summary>
/// サンプルユースケースのインターフェース
/// </summary>
public interface ISampleUseCase
{
    Task<SampleResponse> ExecuteAsync(SampleRequest request, CancellationToken cancellationToken = default);
}
