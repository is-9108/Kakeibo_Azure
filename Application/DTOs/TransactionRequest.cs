using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.DTOs
{
    public class TransactionRequest
    {
        public record CreateTransactionRequest(int CategoryId, string Memo, int Amount,DateTime Date);
        public record UpdateTransactionRequest(int Id, int CategoryId, string Memo, int Amount, DateTime Date);

    }
}
