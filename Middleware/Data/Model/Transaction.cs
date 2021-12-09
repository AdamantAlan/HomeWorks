﻿using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Data.Model
{
    [Table("Transaction")]
    public class Transaction : IEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CardId { get; set; }

        [Required]
        public long UserId { get; set; }

        public TransactionType? Operation { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DateOfTransaction { get; set; }
    }
}