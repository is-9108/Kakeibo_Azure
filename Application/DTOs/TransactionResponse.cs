using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.DTOs
{
    public record TransactionResponse(int id, string Category, string Memo, int Amount, DateTime Date);
    
}
