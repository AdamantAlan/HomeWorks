namespace Middleware.Data
{
    public class KestrelOptions
    {
        public long MaxConcurrentConnections { get; set; }
        public double KeepAliveTimeout { get; set; }
        public long MaxConcurrentUpgradedConnections { get; set; }
        public long MaxRequestBodySize { get; set; }
    }
}
