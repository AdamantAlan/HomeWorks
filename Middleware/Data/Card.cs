using Middleware.Validations.Model;
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
        [MinLength(13)]
        [MaxLength(16)]
        [CardPanValidation(ErrorMessage ="No currect pan.")]
        public string Pan { get; set; }

        [Required]
        [CardDateExpireValidation(ErrorMessage ="Your card is expired.")]
        public DateTime Expire { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public long UserId { get; set; }
    }
}
