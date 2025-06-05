using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Handlers;
using ModularKitchenDesigner.Domain.Interfaces.Logging;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly IExceptionHandlerService _exceptionHandlerService = null!;
        private readonly ILogService _logService = null!;

        public AuditMiddleware(RequestDelegate next, IExceptionHandlerService exceptionHandlerService, ILogService logService)
        {
            _next = next;
            _exceptionHandlerService = exceptionHandlerService;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering(); // Позволяет читать тело запроса несколько раз

            var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();

            httpContext.Request.Body.Position = 0;

            var logResult = await _logService.LogAsync(httpContext, requestBody);

            if (!logResult.IsSuccess)
                await _exceptionHandlerService.ExceptionHandle(httpContext, new ValidationException(logResult.ErrorMessage));


            var originalBodyStream = httpContext.Response.Body;

            await using var memoryStream = new MemoryStream();
                
            httpContext.Response.Body = memoryStream;

            await _next(httpContext);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();

            memoryStream.Seek(0, SeekOrigin.Begin);

            httpContext.Response.Body = originalBodyStream;

            await httpContext.Response.Body.WriteAsync(memoryStream.ToArray());
                
            logResult = await _logService.LogAsync(httpContext, responseBodyText);

            if (!logResult.IsSuccess)
                await _exceptionHandlerService.ExceptionHandle(httpContext, new ValidationException(logResult.ErrorMessage));
        }
    }
}
