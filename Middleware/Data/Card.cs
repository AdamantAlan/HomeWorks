using System;

namespace Middleware.Data
{
    /// <summary>
    /// Модель карты пользователя.
    /// </summary>
    public class Card
    {
        public int Cvc { get; set; }

        public long Pan { get; set; }

        public DateTime Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public long UserId { get; set; }
    }
}
