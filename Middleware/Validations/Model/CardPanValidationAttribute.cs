using System.ComponentModel.DataAnnotations;

namespace Middleware.Validations.Model
{
    /// <summary>
    ///  Validation pan.
    /// </summary>
    public class CardPanValidationAttribute : ValidationAttribute
    {
        public CardPanValidationAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            return Luna(value.ToString());
        }

        private bool Luna(string pan)
        {
            int sum = 0;

            for (int i = 0; i < pan.Length; i++)
            {
                var number = int.Parse(pan[i].ToString());

                if (i % 2 == 0)
                {
                    number *= 2;

                    if (number > 9)
                    {
                        number -= 9;
                    }
                }

                sum += number;
            }

            return sum % 10 == 0 ? true : false;
        }
    }
}
