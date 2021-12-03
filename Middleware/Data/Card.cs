using Middleware.Interfaces;
using Middleware.Validations.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Middleware.Data
{
    /// <summary>
    /// Model user card.
    /// </summary>
    [Table("UserCards")]
    public class Card : IEntity
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [StringLength(3)]
        public string Cvc { get; set; }

        [Required]
        [MinLength(13)]
        [MaxLength(16)]
        [CardPanValidation(ErrorMessage = "No currect pan.")]
        public string Pan { get; set; }

        [Required]
        [CardDateExpireValidation(ErrorMessage = "Your card is expired.")]
        public DateTime Expire { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}
