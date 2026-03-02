using System.Net;
using System.Text.Json;
using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Kakeibo.Api;

/// <summary>
/// サンプル HTTP Trigger（クリーンアーキテクチャのエントリポイント）
/// </summary>
public class SampleFunction
{
    private readonly ISampleUseCase _sampleUseCase;
    private readonly IGetSamplesUseCase _getSamplesUseCase;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public SampleFunction(ISampleUseCase sampleUseCase, IGetSamplesUseCase getSamplesUseCase)
    {
        _sampleUseCase = sampleUseCase;
        _getSamplesUseCase = getSamplesUseCase;
    }

    [Function("GetSamples")]
    public async Task<HttpResponseData> GetSamples(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sample")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var items = await _getSamplesUseCase.ExecuteAsync(cancellationToken);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(items, cancellationToken);
        return response;
    }

    [Function("Sample")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sample")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        SampleRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<SampleRequest>(req.Body, JsonOptions, cancellationToken);
        }
        catch
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
            return badRequest;
        }

        if (request == null || string.IsNullOrWhiteSpace(request.Name))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "Name is required." }, cancellationToken);
            return badRequest;
        }

        var responseDto = await _sampleUseCase.ExecuteAsync(request, cancellationToken);
        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(responseDto, cancellationToken);
        return response;
    }
}
