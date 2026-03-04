using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using static Kakeibo.Application.DTOs.TransactionRequest;

namespace Kakeibo.Api
{
    public class KakeiboFunction
    {
        private readonly ICategory _category;
        private readonly ITransaction _transaction;

        public KakeiboFunction(ICategory category, ITransaction transaction)
        {
            _category = category;
            _transaction = transaction;
        }

        [Function("GetAllCategories")]
        public async Task<HttpResponseData> GetAllCategories(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "api/getAllCategories")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            IReadOnlyCollection<CategoryResponse>? categories;
            try
            {
                categories = await _category.GetAllCategoriesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべてのカテゴリを取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (categories == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "カテゴリが見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(categories, cancellationToken);
            return response;
        }

        [Function("GetAllTransactions")]
        public async Task<HttpResponseData> GetAllTransactions(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "api/getAllTransactions")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TransactionResponse>? transactions;

            try
            {
                transactions = await _transaction.GetAllTransactionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべての取引を取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (transactions == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transactions, cancellationToken);
            return response;
        }
        [Function("AddTransaction")]
        public async Task<HttpResponseData> AddTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "api/addTransaction")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            CreateTransactionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<CreateTransactionRequest>(
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

            await _transaction.AddTransactionAsync(request, cancellationToken);
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }

        [Function("SearchTransaction")]
        public async Task<HttpResponseData> SearchTransaction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "api/searchTransaction/{id}")] HttpRequestData req,
            string id,
            CancellationToken cancellationToken)
        {
            if (!int.TryParse(id, out var transactionId))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { error = "id は数値である必要があります。" }, cancellationToken);
                return badResponse;
            }

            IReadOnlyList<TransactionResponse> transactions;
            try
            {
                transactions = await _transaction.SearchTransactionAsync(transactionId, cancellationToken);
            }
            catch (Exception ex)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "検索中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transactions, cancellationToken);
            return response;
        }
        [Function("UpdateTransaction")]
        public async Task<HttpResponseData> UpdateTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "api/updateTransaction")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            UpdateTransactionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<UpdateTransactionRequest>(
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

            await _transaction.UpdateTransactionAsync(request, cancellationToken);
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
