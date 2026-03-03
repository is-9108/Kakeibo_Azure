using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Domain.Entities
{
    public class UserEntity
    {
        [Key]
        [MaxLength(256)]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
