using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Services
{
    public class MonthlySummary : ISummary
    {
        private readonly ISummary _summary;

        public MonthlySummary(ISummary summary)
        {
            _summary = summary;
        }

        public async Task CreateMonthlySummayAsync(CancellationToken cancellationToken = default)
        {
            await _summary.CreateMonthlySummayAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MonthlySummaryResponse>> GetMonthlySummarieAsync(CancellationToken cancellationToken = default)
        {
            return await _summary.GetMonthlySummarieAsync(cancellationToken);
        }
    }
}
