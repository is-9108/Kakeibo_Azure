using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Domain.Entities
{
    public class MonthlySummaryEntity
    {
        [Key]
        public string Month { get; set; } = string.Empty;
        public int Shuusi { get; set; }

        //支出(IsIncome = false)
        public int Shokuhi { get; set; }
        public int Gaishokuhi { get; set; }
        public int Kounetsuhi { get; set; }
        public int Tsuusinhi { get; set; }
        public int Suidouhi { get; set; }
        public int Koutsuhi { get; set; }
        public int Iryouhi { get; set; }
        public int Zeikin { get; set; }
        public int Yachin { get; set; }
        public int Subscription { get; set; }
        public int Sonota { get; set; }

        //収入(IsIncome = true)
        public int Kyuryo { get; set; }
        public int RinjiShunyu { get; set; }

    }
}
