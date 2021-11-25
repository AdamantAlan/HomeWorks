using Middleware.Data;

namespace Middleware.Dto
{
    /// <summary>
    /// Dto для чтения из "БД"
    /// </summary>
    public class CardReadDto
    {
        public string Pan { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public StatusCard StatusCard { get; set; }
    }
}
