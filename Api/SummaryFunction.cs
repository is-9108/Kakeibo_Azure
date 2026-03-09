using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kakeibo.Api;

public class SummaryFunction
{
    private readonly ISummary _summary;
    private readonly ILogger<SummaryFunction> _logger;

    public SummaryFunction(ISummary summary, ILogger<SummaryFunction> logger)
    {
        _summary = summary;
        _logger = logger;
    }

    [Function("GetAllSummay")]
    public async Task<HttpResponseData> GetAllSummay(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllSummay")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetAllSummay 開始");
        IReadOnlyList<MonthlySummaryResponse>? summaries;
        try
        {
            summaries = await _summary.GetMonthlySummarieAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllSummay: 月次集計一覧の取得に失敗しました");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new { error = "月次集計の取得中に問題が発生しました", details = ex.Message }, cancellationToken);
            return errorResponse;
        }

        if (summaries == null)
        {
            _logger.LogWarning("GetAllSummay: 月次集計が null で返されました");
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteAsJsonAsync(new { error = "月次集計の取得に失敗しました" }, cancellationToken);
            return notFoundResponse;
        }
        _logger.LogInformation("GetAllSummay 成功 件数: {Count}", summaries.Count);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(summaries, cancellationToken);
        return response;
    }
    [Function("CreateSummay")]
    public async Task<HttpResponseData> CreateSummay(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createSummay")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("CreateSummay 開始");
        try
        {
            await _summary.CreateMonthlySummayAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateSummay: 月次集計の作成に失敗しました");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new { error = "月次集計の取得中に問題が発生しました", details = ex.Message }, cancellationToken);
            return errorResponse;
        }

        _logger.LogInformation("CreateSummay 成功");
        var response = req.CreateResponse(HttpStatusCode.Created);
        return response;
    }
}