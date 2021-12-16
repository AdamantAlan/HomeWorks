using System;
using System.ComponentModel.DataAnnotations;

namespace Validations.Validations.Model
{
    /// <summary>
    ///  Validation date expire.
    /// </summary>
    public class CardDateExpireValidationAttribute : ValidationAttribute
    {
        public CardDateExpireValidationAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            if (!(value is DateTime))
            {
                return false;
            }

            var card = (DateTime)value;

            if (card.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }
    }
}
