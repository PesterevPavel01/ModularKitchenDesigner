using System.Text;
using Microsoft.AspNetCore.Http;
using ModularKitchenDesigner.Domain.Interfaces.Logging;
using Result;
using Serilog;

namespace ModularKitchenDesigner.Application.Services.Logging
{
    internal class LogService : ILogService
    {
        private readonly ILogger _logger;
        public LogService( ILogger logger)
        {
            _logger = logger;
        }
        public async Task<BaseResult> LogAsync(HttpContext httpContext, String body)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append("Объект: ");
            
            stringBuilder.AppendLine(httpContext.Response.HasStarted ? "RESPONSE": "REQUEST" );

            stringBuilder.Append("Тип запроса: ");
            stringBuilder.AppendLine(httpContext.Request.Method);

            stringBuilder.Append("Тело запроса: ");
            stringBuilder.AppendLine(body);

            try
            {
                _logger.Information(stringBuilder.ToString());
            }
            catch (Exception exception)
            {
                return new()
                {
                    ErrorMessage = $"Ошибка в LogService: {exception.Message}",
                };
            }

            return new();
        }

        public async Task<BaseResult> LogErrorAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Request.EnableBuffering(); // Позволяет читать тело запроса несколько раз
            var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            httpContext.Request.Body.Position = 0;

            StringBuilder stringBuilder = new();

            stringBuilder.Append("ERROR: ");
            stringBuilder.AppendLine(exception.Message);

            try
            {
                _logger.Error(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                return new()
                {
                    ErrorMessage = $"Ошибка в LogService: {ex.Message}",
                };
            }

            return new();
        }
    }
}
