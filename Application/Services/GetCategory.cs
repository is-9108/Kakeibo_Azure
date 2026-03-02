using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;

namespace Kakeibo.Application.Services
{
    public class GetCategory : ICategory
    {
        private readonly ICategory _category;
        public GetCategory(ICategory category)
        {
            _category = category;
        }

        public async Task<IReadOnlyList<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _category.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
