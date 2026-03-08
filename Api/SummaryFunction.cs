using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kakeibo.Api;

public class SummaryFunction
{
    private readonly ISummary _summary;

    public SummaryFunction(ISummary summary)
    {
        _summary = summary;
    }

    [Function("GetAllSummay")]
    public async Task<HttpResponseData> GetAllSummay(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllSummay")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<MonthlySummaryResponse>? summaries;
        try
        {
            summaries = await _summary.GetMonthlySummarieAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new { error = "月次集計の取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
            return errorResponse;
        }

        if (summaries == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteAsJsonAsync(new { error = "月次集計の取得に失敗しました。" }, cancellationToken);
            return notFoundResponse;
        }
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(summaries, cancellationToken);
        return response;
    }
    [Function("CreateSummay")]
    public async Task<HttpResponseData> CreateSummay(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createSummay")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        try
        {
            await _summary.CreateMonthlySummayAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new { error = "月次集計の取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
            return errorResponse;
        }
        
        var response = req.CreateResponse(HttpStatusCode.Created);
        return response;
    }
}