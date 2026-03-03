using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Domain.Entities
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        [MaxLength(256)]
        public string? Memo { get; set; }

        public UserEntity User { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
