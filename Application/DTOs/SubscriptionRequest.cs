using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.DTOs
{
    public record CreateSubscriptionRequest(string Name, int Amount);
    public record UpdateSubscriptionRequest(int Id, string Name, int Amount);

}
