using Middleware.Data;

namespace Middleware.Dto
{
    /// <summary>
    /// Dto ответа записи в "БД".
    /// </summary>
    public class CardWriteDto
    {
        public long Pan { get; set; }

        public string Name { get; set; }

        public StatusCard StatusCard { get; set; }
    }
}
