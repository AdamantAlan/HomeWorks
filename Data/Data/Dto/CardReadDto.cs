namespace Data.Dto
{
    /// <summary>
    /// Dto for read data from database.
    /// </summary>
    public class CardReadDto
    {
        public long Id { get; set; }

        public string Pan { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public StatusCard StatusCard { get; set; }
    }
}
