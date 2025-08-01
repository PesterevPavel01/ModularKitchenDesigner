﻿using System.Net;
using Microsoft.AspNetCore.Http;
using ModularKitchenDesigner.Application.Errors;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Handlers;
using ModularKitchenDesigner.Domain.Interfaces.Logging;
using Newtonsoft.Json;
using Result;
using TelegramService.Interfaces;

namespace ModularKitchenDesigner.Application.Handlers
{
    public class ExceptionHandlerService : IExceptionHandlerService
    {
        private readonly ITelegramService _telegramService;
        private readonly ILogService _logService;
        public ExceptionHandlerService(ITelegramService telegramService, ILogService logService)
        {
            _telegramService = telegramService;
            _logService = logService;
        }
        public async Task ExceptionHandle(HttpContext httpContext, Exception exception)
        {
            var logResult = await _logService.LogErrorAsync(httpContext, exception);

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

            try 
            {
                await _telegramService.SendMessageAsync(exception.Message);        
            }
            catch (Exception ex) 
            {
                await _logService.LogErrorAsync(httpContext, ex);
            }

            if (!httpContext.Response.HasStarted && httpContext.Response.Body.CanWrite)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
        }
    }
}
