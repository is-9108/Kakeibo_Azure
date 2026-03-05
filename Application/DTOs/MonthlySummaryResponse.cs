using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.DTOs
{
    public record MonthlySummaryResponse(
            string Month,
            int Shuusi,
            int Shokuhi,
            int Gaishokuhi,
            int Kounetsuhi,
            int Tsuusinhi,
            int Suidouhi,
            int Koutsuhi,
            int Iryouhi,
            int Zeikin,
            int Yachin,
            int Subscription,
            int Sonota,
            int Kyuryo,
            int RinjiShunyu
        );
}
