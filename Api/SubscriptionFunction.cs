using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Kakeibo.Api
{
    public class SubscriptionFunction
    {
        private readonly ISubscription _subscription;
        private readonly ILogger<SubscriptionFunction> _logger;

        public SubscriptionFunction(ICategory category, ITransaction transaction, ISubscription subscription, ILogger<SubscriptionFunction> logger)
        {
            _subscription = subscription;
            _logger = logger;
        }

        [Function("GetAllSubscriptions")]
        public async Task<HttpResponseData> GetAllSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllSubscriptions 開始");
            IReadOnlyList<SubscriptionResponse>? subscriptionResponse;
            try
            {
                subscriptionResponse = await _subscription.GetAllSubscriptionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllSubscriptions: サブスク一覧の取得に失敗しました");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべての取引を取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (subscriptionResponse == null)
            {
                _logger.LogWarning("GetAllSubscriptions: サブスクが null で返されました");
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }
            _logger.LogInformation("GetAllSubscriptions 成功 件数: {Count}", subscriptionResponse.Count);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(subscriptionResponse, cancellationToken);
            return response;

        }
        [Function("CreateSubscriptions")]
        public async Task<HttpResponseData> CreateSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateSubscriptions 開始");
            CreateSubscriptionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<CreateSubscriptionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CreateSubscriptions: リクエストボディのパースに失敗しました");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                _logger.LogWarning("CreateSubscriptions: リクエストボディが空です");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _subscription.CreateSubscriptionsAsync(request, cancellationToken);
            _logger.LogInformation("CreateSubscriptions 成功");
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;

        }
        [Function("UpdateSubscriptions")]
        public async Task<HttpResponseData> UpdateSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateSubscriptions 開始");
            UpdateSubscriptionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<UpdateSubscriptionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "UpdateSubscriptions: リクエストボディのパースに失敗しました");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                _logger.LogWarning("UpdateSubscriptions: リクエストボディが空です");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _subscription.UpdateSubscriptionsAsync(request, cancellationToken);
            _logger.LogInformation("UpdateSubscriptions 成功 id: {Id}", request.Id);
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
        [Function("DeleteSubscriptions")]
        public async Task<HttpResponseData> DeleteSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteSubscriptions/{id}")] HttpRequestData req,
           int id,
           CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteSubscriptions 開始 id: {Id}", id);
            try
            {
                await _subscription.DeleteSubscriptionsAsync(id, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning("DeleteSubscriptions: サブスクが見つかりませんでした id: {Id}", id);
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteSubscriptions: 削除に失敗しました id: {Id}", id);
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "削除中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            _logger.LogInformation("DeleteSubscriptions 成功 id: {Id}", id);
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
