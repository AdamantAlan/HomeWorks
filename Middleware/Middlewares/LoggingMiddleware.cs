using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Middlewares
{
    /// <summary>
    /// Middleware for logging request/response.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly StringBuilder _log;
        private ServerInfo _info;

        public LoggingMiddleware(RequestDelegate next, ServerInfo info)
        {
            _next = next;
            _info = info;
            _log = new StringBuilder();
        }

        public async Task InvokeAsync(HttpContext context, ILogger<LoggingMiddleware> log)
        {
            await _next.Invoke(context);
            _log.AppendLine($"------Request info");
            _log.AppendLine($"Path:{context.Request.Path.Value.ToLower()}");
            _log.AppendLine($"Method:{context.Request.Method.ToUpper()}");
            _log.AppendLine($"Protocol:{context.Request.Protocol}");
            _log.AppendLine($"Date time:{DateTimeOffset.UtcNow}");
            _log.AppendLine($"------Response info");
            _log.AppendLine($"Path:{context.Response.StatusCode}");
            _log.AppendLine($"ContentType:{context.Response.ContentType ?? "null"}");
            _log.AppendLine($"ContentLength:{context.Response.ContentLength ?? 0}");
            _log.AppendLine($"------Server info");
            _log.AppendLine($"Server:{_info.server}");
            _log.AppendLine($"ENV:{_info.env}");
            _log.AppendLine($"VS:{_info.vs}");

            log.LogInformation(_log.ToString());

            _log.Clear();
        }

    }
}
