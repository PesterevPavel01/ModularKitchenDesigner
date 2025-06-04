using System;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Handlers;
using ModularKitchenDesigner.Domain.Interfaces.Logging;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class ExcertionHandlingMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly IExceptionHandlerService _exceptionHandlerService = null!;
        private readonly ILogService _logService = null!;

        public ExcertionHandlingMiddleware(RequestDelegate next, IExceptionHandlerService exceptionHandlerService, ILogService logService)
        {
            _next = next;
            _exceptionHandlerService = exceptionHandlerService;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            var logResult = await _logService.LogAsync(httpContext);

            if (!logResult.IsSuccess)
                await _exceptionHandlerService.ExceptionHandle(httpContext, new ValidationException(logResult.ErrorMessage));

            try 
            { 
                await _next(httpContext);
            }
            catch (Exception exception) 
            {
                await _exceptionHandlerService.ExceptionHandle(httpContext, exception);
            }
        }
    }
}
