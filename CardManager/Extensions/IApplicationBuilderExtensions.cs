using Data;
using Microsoft.AspNetCore.Builder;
using Data;
using CardManager.Middlewares;

namespace CardManager.Extensions
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
