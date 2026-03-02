using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Kakeibo.Api
{
    public class KakeiboFunction
    {
        private readonly ICategory _category;

        public KakeiboFunction(ICategory category)
        {
            _category = category;
        }

        [Function("GetAllCategories")]
        public async Task<HttpResponseData> GetAllCategories(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getAllCategories")] HttpRequestData req,
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

            if(categories == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "カテゴリが見つかりませんでした。" }, cancellationToken);
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(categories, cancellationToken);
            return response;
        }
    }
}
