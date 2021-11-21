using Microsoft.AspNetCore.Builder;
using Middleware.Data;
using Middleware.Middlewares;

namespace Middleware.Extensions
{

    static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Метод расширения для более читабельного вызова middleware.
        /// </summary>
        public static IApplicationBuilder Logging(this IApplicationBuilder builder, ServerInfo info)
        {
            return builder.UseMiddleware<LoggingMiddleware>(info);
        }
    }
}
