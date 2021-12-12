using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Data
{
    /// <summary>
    /// The type of transaction in the operation.
    /// </summary>
    public enum TransactionType
    {
        deposit,
        removal,
        payOnline,

        /// <summary>
        /// Transfer card to card.
        /// </summary>
        transferС2C
    }
}
