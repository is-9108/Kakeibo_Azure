using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kakeibo.Application.DTOs.TransactionRequest;

namespace Kakeibo.Application.Services
{
    public class Transaction : ITransaction
    {
        private readonly ITransaction _transaction;

        public Transaction(ITransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task AddTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default)
        {
            await _transaction.AddTransactionAsync(request, cancellationToken);
            return;
        }

        public async Task<IReadOnlyList<TransactionResponse>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return await _transaction.GetAllTransactionsAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TransactionResponse>> SearchTransactionAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _transaction.SearchTransactionAsync(id, cancellationToken);
        }

        public async Task UpdateTransactionAsync(UpdateTransactionRequest request, CancellationToken cancellationToken = default)
        {
             await _transaction.UpdateTransactionAsync(request, cancellationToken);
        }
    }
}
