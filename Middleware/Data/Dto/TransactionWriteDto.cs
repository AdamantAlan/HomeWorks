using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Data.Dto
{
    public class TransactionWriteDto
    {
        [Required]
        public long CardId { get; set; }

        [Required]
        public long UserId { get; set; }

        public TransactionType? Operation { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
