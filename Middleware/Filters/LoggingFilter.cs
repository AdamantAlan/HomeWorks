using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Middleware.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingFilter> _log;
        private readonly StringBuilder _trace;
        public LoggingFilter(ILogger<LoggingFilter> log)
        {
            _log = log;
            _trace = new();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _trace.AppendLine("------Request data");

            _trace.AppendLine("------Query values");
            var querys = context.HttpContext.Request.Query.ToList();
            foreach (var q in querys)
            {
                _trace.AppendLine($"Key={q.Key} value={q.Value}");
            }

            _trace.AppendLine("------Route values");
            var routers = context.HttpContext.Request.RouteValues.ToList();
            foreach (var r in routers)
            {
                _trace.AppendLine($"Key={r.Key} value={r.Value}");
            }

            // No currect work!
            using var stream = new StreamReader(context.HttpContext.Request.Body);
            var body = await stream.ReadToEndAsync();

            _trace.AppendLine("------Body value");
            _trace.AppendLine(body);

            _log.LogInformation(_trace.ToString());

            var executedContext = await next();

            _trace.Clear();
            _trace.AppendLine("------Response data");

            var result = executedContext.Result as ObjectResult;
            _trace.AppendLine($"Response code: {result.StatusCode}");

            if (result.Value is ResultApi values)
            {
                var valuesResult = values.Result ?? "No data response";
                _trace.AppendLine($"Data: {JsonSerializer.Serialize(valuesResult)}");
                _trace.AppendLine($"ErrorCode: {values.ErrorCode}");
                _trace.AppendLine($"ErrorMessage: {values.ErrorMessage ?? "No errors"}");
            }

            _log.LogInformation(_trace.ToString());
        }
    }
}
