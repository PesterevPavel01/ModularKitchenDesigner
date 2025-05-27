using System.Net;
using Microsoft.AspNetCore.Http;
using ModularKitchenDesigner.Application.Errors;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Handlers;
using Newtonsoft.Json;
using Result;
using TelegramService.Interfaces;

namespace ModularKitchenDesigner.Application.Handlers
{
    public class ExceptionHandlerService : IExceptionHandlerService
    {
        private readonly ITelegramService _telegramService;
        public ExceptionHandlerService(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }
        public async Task ExceptionHandle(HttpContext httpContext, Exception exception)
        {
            var code = exception switch
            {
                ValidationException =>  JsonConvert.DeserializeObject<ErrorMessage>(exception.Message)?.Code ?? (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result =
                new BaseResult
                { 
                    ErrorMessage = exception.Message,
                    ErrorCode = code,
                    ConnectionTime = DateTime.UtcNow,
                };

            await _telegramService.SendMessageAsync(exception.Message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)result.ErrorCode;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
