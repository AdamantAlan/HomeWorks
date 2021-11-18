namespace Middleware.Data
{
    /// <summary>
    /// Структура для записи переменных окружения.
    /// </summary>
    public struct ServerInfo
    {
        internal string vs { get; set; }
        internal string env { get; set; }
        internal string server { get; set; }
    }
}
