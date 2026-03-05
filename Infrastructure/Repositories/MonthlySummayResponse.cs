using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;
using Kakeibo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Kakeibo.Infrastructure.Repositories
{
    public class MonthlySummayResponse : ISummary
    {
        private readonly AppDbContext _context;

        public MonthlySummayResponse(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMonthlySummaysync(CancellationToken cancellationToken = default)
        {
            var transactions = await _context.Transactions.Include(t => t.Category).ToListAsync(cancellationToken);

            var month = DateTime.Now.ToString("yyyy-MM");

            //収入合計
            var totalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            //支出合計
            var totalExpense = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);
            var shuusi = totalIncome - totalExpense;

            var byCategory = transactions
                .GroupBy(t => t.Category.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();

            var monthlySummary = new MonthlySummaryEntity
            {
                Month = month,
                Shuusi = shuusi,
                Shokuhi = byCategory.Where(c => c.CategoryName == "食費").Sum(c => c.TotalAmount),
                Gaishokuhi = byCategory.Where(c => c.CategoryName == "外食費").Sum(c => c.TotalAmount),
                Kounetsuhi = byCategory.Where(c => c.CategoryName == "光熱費").Sum(c => c.TotalAmount),
                Tsuusinhi = byCategory.Where(c => c.CategoryName == "通信費").Sum(c => c.TotalAmount),
                Suidouhi = byCategory.Where(c => c.CategoryName == "水道費").Sum(c => c.TotalAmount),
                Koutsuhi = byCategory.Where(c => c.CategoryName == "交通費").Sum(c => c.TotalAmount),
                Iryouhi = byCategory.Where(c => c.CategoryName == "医療費").Sum(c => c.TotalAmount),
                Zeikin = byCategory.Where(c => c.CategoryName == "税金").Sum(c => c.TotalAmount),
                Yachin = byCategory.Where(c => c.CategoryName == "家賃").Sum(c => c.TotalAmount),
                Subscription = byCategory.Where(c => c.CategoryName == "サブスク").Sum(c => c.TotalAmount),
                Sonota = byCategory.Where(c => c.CategoryName == "その他").Sum(c => c.TotalAmount),
                Kyuryo = byCategory.Where(c => c.CategoryName == "給料").Sum(c => c.TotalAmount),
                RinjiShunyu = byCategory.Where(c => c.CategoryName == "臨時給料").Sum(c => c.TotalAmount)
            };

            await _context.MonthlySummaries.AddAsync(monthlySummary, cancellationToken);

            await _context.Transactions.ExecuteDeleteAsync(cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MonthlySummaryResponse>> GetMonthlySummarieAsync(CancellationToken cancellationToken = default)
        {
            var summaries = await _context.MonthlySummaries.ToListAsync(cancellationToken);
            return summaries
                .Select(s => new MonthlySummaryResponse
                (
                    s.Month,
                    s.Shuusi,
                    s.Shokuhi,
                    s.Gaishokuhi,
                    s.Kounetsuhi,
                    s.Tsuusinhi,
                    s.Suidouhi,
                    s.Koutsuhi,
                    s.Iryouhi,
                    s.Zeikin,
                    s.Yachin,
                    s.Subscription,
                    s.Sonota,
                    s.Kyuryo,
                    s.RinjiShunyu
                ))
                .ToList();
        }
    }
}
