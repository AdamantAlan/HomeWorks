using System;
using System.ComponentModel.DataAnnotations;

namespace Middleware.Data
{
    /// <summary>
    /// Model user card.
    /// </summary>
    public class Card
    {
        [Required]
        public string Cvc { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(13)]
        public string Pan { get; set; }

        [Required]
        public DateTime Expire { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public long UserId { get; set; }
    }
}
