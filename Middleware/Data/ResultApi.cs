namespace Middleware.Data
{
    /// <summary>
    /// Result work server.
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    public class ResultApi<T>
    {
        public T Result { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
