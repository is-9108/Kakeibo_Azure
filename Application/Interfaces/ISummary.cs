using Kakeibo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Interfaces
{
    public interface ISummary
    {
        Task<IReadOnlyList<MonthlySummaryResponse>> GetMonthlySummarieAsync(CancellationToken cancellationToken = default);
        Task CreateMonthlySummayAsync(CancellationToken cancellationToken = default);

    }
}
