using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kakeibo.Api
{
    public class CategoryFunction
    {
        private readonly ICategory _category;
        private readonly ILogger<CategoryFunction> _logger;

        public CategoryFunction(ICategory category, ILogger<CategoryFunction> logger)
        {
            _category = category;
            _logger = logger;
        }

        [Function("GetAllCategories")]
        public async Task<HttpResponseData> GetAllCategories(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllCategories")] HttpRequestData req,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllCategories 開始");
            IReadOnlyCollection<CategoryResponse>? categories;
            try
            {
                categories = await _category.GetAllCategoriesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllCategories: カテゴリ一覧の取得に失敗しました");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(new { error = "すべてのカテゴリを取得中に問題が発生しました。", details = ex.Message }, cancellationToken);
                return errorResponse;
            }

            if (categories == null)
            {
                _logger.LogWarning("GetAllCategories: カテゴリが null で返されました");
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "カテゴリが見つかりませんでした" }, cancellationToken);
                return notFoundResponse;
            }

            _logger.LogInformation("GetAllCategories 成功 件数: {Count}", categories.Count);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(categories, cancellationToken);
            return response;
        }
    }
}

