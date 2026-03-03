using Kakeibo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kakeibo.Application.DTOs.TransactionRequest;

namespace Kakeibo.Application.Interfaces
{
    public interface ITransaction
    {
        Task<IReadOnlyList<TransactionResponse>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        Task AddTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default);
    }
}
