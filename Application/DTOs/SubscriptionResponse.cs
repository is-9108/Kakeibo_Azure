using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.DTOs
{
    public record SubscriptionResponse(int Id, string Name, int Amount);
}
