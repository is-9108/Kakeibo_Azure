using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using static Kakeibo.Application.DTOs.TransactionRequest;

namespace Kakeibo.Api
{
    public class TransactionFunction
    {
        private readonly ITransaction _transaction;
        private readonly ILogger<TransactionFunction> _logger;

        public TransactionFunction(ITransaction transaction, ILogger<TransactionFunction> logger)
        {
            _transaction = transaction;
            _logger = logger;
        }


        [Function("GetAllTransactions")]
        public async Task<HttpResponseData> GetAllTransactions(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllTransactions")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllTransactions 開始");
            IReadOnlyCollection<TransactionResponse>? transactions;

            try
            {
                transactions = await _transaction.GetAllTransactionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllTransactions: 取引一覧の取得に失敗しました");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべての取引を取得中に問題が発生しました", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (transactions == null)
            {
                _logger.LogWarning("GetAllTransactions: 取引が null で返されました");
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした" }, cancellationToken);
                return notFoundResponse;
            }
            _logger.LogInformation("GetAllTransactions 成功 件数: {Count}", transactions.Count);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transactions, cancellationToken);
            return response;
        }
        [Function("AddTransaction")]
        public async Task<HttpResponseData> AddTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addTransaction")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("AddTransaction 開始");
            CreateTransactionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<CreateTransactionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "AddTransaction: リクエストボディのパースに失敗しました");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                _logger.LogWarning("AddTransaction: リクエストボディが空です");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _transaction.AddTransactionAsync(request, cancellationToken);
            _logger.LogInformation("AddTransaction 成功");
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }

        [Function("SearchTransaction")]
        public async Task<HttpResponseData> SearchTransaction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "searchTransaction/{id}")] HttpRequestData req,
            string id,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("SearchTransaction 開始 id: {Id}", id);
            if (!int.TryParse(id, out var transactionId))
            {
                _logger.LogWarning("SearchTransaction: 無効な id が指定されました id: {Id}", id);
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { error = "id は数値である必要があります" }, cancellationToken);
                return badResponse;
            }

            IReadOnlyList<TransactionResponse> transactions;
            try
            {
                transactions = await _transaction.SearchTransactionAsync(transactionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchTransaction: 検索に失敗しました id: {TransactionId}", transactionId);
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "検索中に問題が発生しました", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            _logger.LogInformation("SearchTransaction 成功 id: {TransactionId} 件数: {Count}", transactionId, transactions.Count);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transactions, cancellationToken);
            return response;
        }
        [Function("UpdateTransaction")]
        public async Task<HttpResponseData> UpdateTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateTransaction")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateTransaction 開始");
            UpdateTransactionRequest? request;
            try
            {
                request = await JsonSerializer.DeserializeAsync<UpdateTransactionRequest>(
                    req.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "UpdateTransaction: リクエストボディのパースに失敗しました");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Invalid request body." }, cancellationToken);
                return bad;
            }

            if (request == null)
            {
                _logger.LogWarning("UpdateTransaction: リクエストボディが空です");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteAsJsonAsync(new { error = "Request body is required." }, cancellationToken);
                return bad;
            }

            await _transaction.UpdateTransactionAsync(request, cancellationToken);
            _logger.LogInformation("UpdateTransaction 成功 id: {Id}", request.Id);
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
        [Function("DeleteTransaction")]
        public async Task<HttpResponseData> DeleteTransaction(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteTransaction/{id}")] HttpRequestData req,
            string id,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteTransaction 開始 id: {Id}", id);
            if (!int.TryParse(id, out var transactionId))
            {
                _logger.LogWarning("DeleteTransaction: 無効な id が指定されました id: {Id}", id);
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { error = "id は数値である必要があります" }, cancellationToken);
                return badResponse;
            }

            try
            {
                await _transaction.DeleteTransactionAsync(transactionId, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning("DeleteTransaction: 取引が見つかりませんでした id: {TransactionId}", transactionId);
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "取引が見つかりませんでした" }, cancellationToken);
                return notFoundResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteTransaction: 削除に失敗しました id: {TransactionId}", transactionId);
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "削除中に問題が発生しました", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            _logger.LogInformation("DeleteTransaction 成功 id: {TransactionId}", transactionId);
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        
        [Function("RegisterSubscriptions")]
        public async Task<HttpResponseData> RegisterSubscriptions(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "registerSubscriptions")] HttpRequestData req,
      CancellationToken cancellationToken)
        {
            _logger.LogInformation("RegisterSubscriptions 開始");
            try
            {
                await _transaction.RegisterSubscriptionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterSubscriptions: サブスク登録に失敗しました");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "サブスクの登録中に問題が発生しました", details = ex.Message }, cancellationToken);
                return errorResponse;
            }
            _logger.LogInformation("RegisterSubscriptions 成功");
            var response = req.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
