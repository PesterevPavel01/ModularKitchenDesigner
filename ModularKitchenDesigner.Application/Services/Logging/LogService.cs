using System.Text;
using Microsoft.AspNetCore.Http;
using ModularKitchenDesigner.Domain.Interfaces.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (httpContext.Request.Path.StartsWithSegments("/swagger"))
                return new();

            var formattedRequestBody = string.IsNullOrEmpty(body)
            ? "N/A"
            : JToken.Parse(body).ToString(Formatting.Indented);

            StringBuilder stringBuilder = new();

            stringBuilder.Append(" Объект: ");
            stringBuilder.Append(httpContext.Response.HasStarted ? "RESPONSE;": "REQUEST;" );

            stringBuilder.Append(" Тип запроса: ");
            stringBuilder.Append(httpContext.Request.Method);
            stringBuilder.Append(';');

            if (!string.IsNullOrEmpty(body))
            {
                stringBuilder.AppendLine($" Тело запроса: ");
                stringBuilder.Append($"{formattedRequestBody};");
            }

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
            StringBuilder stringBuilder = new();

            stringBuilder.Append("ERROR: ");
            stringBuilder.AppendLine(
                string.IsNullOrEmpty(exception.Message)
                ? ""
                : JToken.Parse(exception.Message).ToString(Formatting.Indented));

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
