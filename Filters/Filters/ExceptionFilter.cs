using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Data;
using Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filters.Filters
{
    /// <summary>
    /// Filter for catching errors response.
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _log;

        public ExceptionFilter(ILogger<ExceptionFilter> log)
        {
            _log = log;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            _log.LogError($"Response error \n Path {context.HttpContext.Request.Path} \n Error action result {exceptionMessage} \n {exceptionStack}");
            context.Result = new InternalServerErrorObjectResult(new ResultApi<string> { Result = actionName, ErrorCode = 687, ErrorMessage = $"{exceptionMessage}" });
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
