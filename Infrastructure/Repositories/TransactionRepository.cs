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
    public class TransactionRepository : ITransaction
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TransactionResponse>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.transactions
                .Include(t => t.Category) // カテゴリを含める
                .Select(t => new TransactionResponse(
                    t.Id,
                    t.Category.Name,
                    t.Memo,
                    t.Amount,
                    t.Date))
                .ToListAsync(cancellationToken);
        }
    }
}
