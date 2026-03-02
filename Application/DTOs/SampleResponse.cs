namespace Kakeibo.Application.DTOs;

/// <summary>
/// サンプルAPIのレスポンスDTO
/// </summary>
public record SampleResponse(Guid Id, string Name, decimal Amount, DateTime CreatedAt);
