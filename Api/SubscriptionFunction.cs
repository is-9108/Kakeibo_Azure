using Azure.Core;
using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using static Kakeibo.Application.DTOs.TransactionRequest;

namespace Kakeibo.Api
{
    public class SubscriptionFunction
    {
        private readonly ISubscription _subscription;

        public SubscriptionFunction(ICategory category, ITransaction transaction, ISubscription subscription)
        {
            _subscription = subscription;
        }

        [Function("GetAllSubscriptions")]
        public async Task<HttpResponseData> GetAllSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            IReadOnlyList<SubscriptionResponse>? subscriptionResponse;
            try
            {
                subscriptionResponse = await _subscription.GetAllSubscriptionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべての取引を取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (subscriptionResponse == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(subscriptionResponse, cancellationToken);
            return response;

        }
        [Function("CreateSubscriptions")]
        public async Task<HttpResponseData> CreateSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            CreateSubscriptionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<CreateSubscriptionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _subscription.CreateSubscriptionsAsync(request, cancellationToken);
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;

        }
        [Function("UpdateSubscriptions")]
        public async Task<HttpResponseData> UpdateSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateSubscriptions")] HttpRequestData req,
           CancellationToken cancellationToken)
        {
            UpdateSubscriptionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<UpdateSubscriptionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _subscription.UpdateSubscriptionsAsync(request, cancellationToken);
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }
        [Function("DeleteSubscriptions")]
        public async Task<HttpResponseData> DeleteSubscriptions(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteSubscriptions/{id}")] HttpRequestData req,
           int id,
           CancellationToken cancellationToken)
        {
            try
            {
                await _subscription.DeleteSubscriptionsAsync(id, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }
            catch (Exception ex)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "削除中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}

