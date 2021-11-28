using Middleware.Data;
using System;

namespace Middleware.Dto
{
    /// <summary>
    /// Dto for write in database. No used!
    /// </summary>
    [Obsolete("No used")]
    public class CardWriteDto
    {
        public string Pan { get; set; }

        public string Name { get; set; }

        public StatusCard StatusCard { get; set; }
    }
}
