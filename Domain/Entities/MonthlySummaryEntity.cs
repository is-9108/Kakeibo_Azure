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
        public int Shuusi { get; set; } = 0;

        //支出(IsIncome = false)
        public int Shokuhi { get; set; } = 0;
        public int Gaishokuhi { get; set; } = 0;
        public int Kounetsuhi { get; set; } = 0;
        public int Tsuusinhi { get; set; } = 0;
        public int Suidouhi { get; set; } = 0;
        public int Koutsuhi { get; set; } = 0;
        public int Iryouhi { get; set; } = 0;
        public int Zeikin { get; set; } = 0;
        public int Yachin { get; set; } = 0;
        public int Subscription { get; set; } = 0;
        public int Sonota { get; set; } = 0;

        //収入(IsIncome = true)
        public int Kyuryo { get; set; } = 0;
        public int RinjiShunyu { get; set; } = 0;

    }
}
