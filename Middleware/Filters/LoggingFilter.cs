using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using Middleware.Dto;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Middleware.Filters
{
    /// <summary>
    /// Filter for logging request/response.
    /// </summary>
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

            if (context.HttpContext.Request.Method.ToUpper() != "GET")
            {

                var req = context.HttpContext.Request;
                if (req.Body.CanSeek)
                {
                    req.Body.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(
                      req.Body,
                      encoding: Encoding.UTF8,
                      bufferSize: 8192,
                      leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        _trace.AppendLine("------Body value");

                        if (context.HttpContext.Request.Path.Value.Contains(ActionsList.AddCard.ToString()) ||
                           context.HttpContext.Request.Path.Value.Contains(ActionsList.DeleteCard.ToString()))
                        {
                            var card = JsonSerializer.Deserialize<Card>(body);
                            _trace.AppendLine($"CardHolder: {card.Name}");
                            _trace.AppendLine($"Cvc: {GetHiddenCvc(card.Cvc)}");
                            _trace.AppendLine($"Pan: {GetHiddenPan(card.Pan)}");
                        }

                        if (context.HttpContext.Request.Path.Value.Contains(ActionsList.GetCard.ToString()))
                        {
                            var bodyData = JsonSerializer.Deserialize<string>(body);
                            _trace.AppendLine($"Pan: {GetHiddenPan(bodyData)}");
                        }

                        if (context.HttpContext.Request.Path.Value.Contains(ActionsList.ChangeCardHolder.ToString()))
                        {
                            var bodyData = JsonSerializer.Deserialize<string>(body);
                            _trace.AppendLine($"Pan: {bodyData}");
                        }
                    }
                }
            }

            _log.LogInformation(_trace.ToString());

            var executedContext = await next();

            _trace.Clear();
            _trace.AppendLine("------Response data");

            var result = executedContext.Result as ObjectResult;
            _trace.AppendLine($"Response code: {result.StatusCode}");

            if (result.Value is ResultApi values)
            {
                GetHiddenResponseResultData(values.Result ?? "No data response");
            }

            _log.LogInformation(_trace.ToString());
        }

        private string GetHiddenCvc(string cvc) => "***";
        private string GetHiddenPan(string pan) => pan.Replace(pan.Substring(0, pan.Length - 4), new string('*', pan.Length - 4));

        private void GetHiddenResponseResultData(object result)
        {
            StringBuilder sb = new();

            if (result is IEnumerable<CardReadDto> cards)
            {
                foreach (var c in cards)
                {
                    _trace.AppendLine("CardHolder: " + c.Name);
                    _trace.AppendLine("Pan: " + GetHiddenPan(c.Pan));
                    _trace.AppendLine("StatusCard: " + c.StatusCard.ToString());
                    _trace.AppendLine();
                }
            }

            if (result is CardReadDto card)
            {
                _trace.AppendLine("CardHolder: " + card.Name);
                _trace.AppendLine("Pan: " + GetHiddenPan(card.Pan));
                _trace.AppendLine("StatusCard: " + card.StatusCard.ToString());
                _trace.AppendLine();
            }
        }
    }
}
