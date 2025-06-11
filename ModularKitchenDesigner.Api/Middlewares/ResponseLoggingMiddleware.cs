using ModularKitchenDesigner.Domain.Interfaces.Logging;
using TelegramService.Interfaces;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly ILogService _logService = null!;
        private readonly ITelegramService _telegramService;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogService logService, ITelegramService telegramService)
        {
            _next = next;
            _logService = logService;
            _telegramService = telegramService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var originalBodyStream = httpContext.Response.Body;

            try
            {
                using var memoryStream = new MemoryStream();

                httpContext.Response.Body = memoryStream;

                await _next(httpContext);

                memoryStream.Seek(0, SeekOrigin.Begin);

                using var streamReader = new StreamReader(memoryStream);

                var responseBodyText = await streamReader.ReadToEndAsync();

                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBodyStream);

                var logResult = await _logService.LogAsync(httpContext, responseBodyText);
            }
            catch (Exception exception)
            {
                await _telegramService.SendMessageAsync(exception.Message);
            }
            finally
            {
                httpContext.Response.Body = originalBodyStream;
            }

        }
    }
}
