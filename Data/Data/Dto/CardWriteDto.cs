﻿using Validations.Validations.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    /// <summary>
    /// Dto for write in database.
    /// </summary>
    public class CardWriteDto
    {
        public long userId { get; set; }

        [Required]
        [StringLength(3)]
        public string cvc { get; set; }

        [Required]
        [MinLength(13)]
        [MaxLength(16)]
        [CardPanValidation(ErrorMessage = "No currect pan.")]
        public string pan { get; set; }

        [Required]
        [CardDateExpireValidation(ErrorMessage = "Your card is expired.")]
        public DateTime expire { get; set; }

        [Required]
        public string name { get; set; }

        public bool isDefault { get; set; }
    }
}