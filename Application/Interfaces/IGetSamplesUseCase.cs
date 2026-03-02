using Kakeibo.Application.DTOs;

namespace Kakeibo.Application.Interfaces;

/// <summary>
/// サンプル一覧取得ユースケースのインターフェース
/// </summary>
public interface IGetSamplesUseCase
{
    Task<IReadOnlyList<SampleResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}
