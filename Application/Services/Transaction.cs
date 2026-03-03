using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Services
{
    public class Transaction : ITransaction
    {
        private readonly ITransaction _transaction;

        public Transaction(ITransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<IReadOnlyList<TransactionResponse>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return await _transaction.GetAllTransactionsAsync(cancellationToken);
        }
    }
}
