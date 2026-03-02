using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Infrastructure.Repositories
{
    public class CategoryRepository : ICategory
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IReadOnlyList<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _db.Categories.ToListAsync(cancellationToken);
            return entities
                .Select(e => new CategoryResponse(e.Id, e.Name))
                .ToList();
        }
    }
}
