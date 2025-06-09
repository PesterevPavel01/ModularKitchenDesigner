using System;
using ModularKitchenDesigner.Domain.Interfaces.Logging;
using TelegramService;
using TelegramService.Interfaces;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly ILogService _logService = null!;
        private readonly ITelegramService _telegramService;

        public RequestLoggingMiddleware(RequestDelegate next, ILogService logService, ITelegramService telegramService)
        {
            _next = next;
            _logService = logService;
            _telegramService = telegramService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering(); // Позволяет читать тело запроса несколько раз

            using StreamReader streamReader = new(httpContext.Request.Body);

            var requestBody = await streamReader.ReadToEndAsync();

            httpContext.Request.Body.Position = 0;

            try
            {
                var logResult = await _logService.LogAsync(httpContext, requestBody);
            }
            catch (Exception exception)
            {
                await _telegramService.SendMessageAsync(exception.Message);
            }
            
            await _next(httpContext);
        }
    }
}
