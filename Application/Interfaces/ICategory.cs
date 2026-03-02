using Kakeibo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Interfaces
{
    public interface ICategory
    {
        Task<IReadOnlyList<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
