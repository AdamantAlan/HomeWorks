using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using Middleware.Dto;
using Middleware.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
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
            try
            {
                _trace.AppendLine("------Request data");

                LoggingDataRequest(context);
            }
            catch (Exception e)
            {
                _log.LogError($"Error logging request, {e.Message}\n\n{e.StackTrace}\n\n");
            }
            finally
            {
                _log.LogInformation(_trace.ToString());
                _trace.Clear();
            }

            var executedContext = await next();

            try
            {
                _trace.Clear();
                _trace.AppendLine("------Response data");

                var result = executedContext.Result as ObjectResult;
                _trace.AppendLine($"Response code: {result.StatusCode}");

                LoggingResponseResultData(result.Value);
            }
            catch (Exception e)
            {
                _log.LogError($"Error logging response, {e.Message}\n\n{e.StackTrace}\n\n");
            }
            finally
            {
                _log.LogInformation(_trace.ToString());
            }
        }

        private string GetHiddenCvc(string cvc) => cvc.GetHiddenCvc();
        private string GetHiddenPan(string pan) => pan.GetHiddenPan();
        private void ErrorLoggig(string userId, int code, string reason) => _trace.AppendLine($"User id: {userId}").AppendLine($"Error code: {code}").AppendLine($"Error message: {reason}\n");
        private void LoggingResponseResultData(object result)
        {
            if (result is ResultApi<string> error && error.ErrorCode != 0)
            {
                ErrorLoggig(error.Result, error.ErrorCode, error.ErrorMessage);
                return;
            }

            if (result is ResultApi<IEnumerable<CardReadDto>> cards)
            {
                foreach (var c in cards.Result)
                {
                    _trace.AppendLine("CardHolder: " + c.Name);
                    _trace.AppendLine("Pan: " + GetHiddenPan(c.Pan));
                    _trace.AppendLine("StatusCard: " + c.StatusCard.ToString());
                    _trace.AppendLine();
                }

                return;
            }

            if (result is ResultApi<CardReadDto> card)
            {
                _trace.AppendLine("CardHolder: " + card.Result.Name);
                _trace.AppendLine("Pan: " + GetHiddenPan(card.Result.Pan));
                _trace.AppendLine("StatusCard: " + card.Result.StatusCard.ToString());
                _trace.AppendLine();
            }
        }

        private void LoggingDataRequest(ActionExecutingContext context)
        {
            foreach (var item in context.ActionArguments)
            {
                switch (item.Key)
                {
                    case "card":
                        var card = item.Value as Card;
                        _trace.AppendLine($"CardHolder: {card.name}");
                        _trace.AppendLine($"Cvc: {GetHiddenCvc(card.cvc)}");
                        _trace.AppendLine($"Pan: {GetHiddenPan(card.pan)}");
                        break;
                    case "pan":
                        var pan = item.Value.ToString();
                        _trace.AppendLine($"Pan: {GetHiddenPan(pan)}");
                        break;
                    default:
                        _trace.AppendLine($"{item.Key}: {item.Value}");
                        break;
                }
            }
        }
    }
}
