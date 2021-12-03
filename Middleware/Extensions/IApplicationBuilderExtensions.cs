using Microsoft.AspNetCore.Builder;
using Middleware.Data;
using Middleware.Middlewares;

namespace Middleware.Extensions
{

    static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// An extension method for a more readable call to middleware.
        /// </summary>
        public static IApplicationBuilder Logging(this IApplicationBuilder builder, ServerInfo info)
        {
            return builder.UseMiddleware<LoggingMiddleware>(info);
        }
    }
}
