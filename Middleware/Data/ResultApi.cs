namespace Middleware.Data
{
    /// <summary>
    /// Результат отработки логики от сервера.
    /// </summary>
    public class ResultApi
    {
        public object Result { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
