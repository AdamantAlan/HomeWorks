using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Dto
{
    public class TransactionReadDto
    {
        [Required]
        public long CardId { get; set; }

        public TransactionType? Operation { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DateOfTransaction { get; set; }
    }
}
