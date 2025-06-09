using ModularKitchenDesigner.Domain.Interfaces.Handlers;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly IExceptionHandlerService _exceptionHandlerService = null!;

        public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionHandlerService exceptionHandlerService)
        {
            _next = next;
            _exceptionHandlerService = exceptionHandlerService;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try 
            { 
                await _next(httpContext);
            }
            catch (Exception exception) 
            {
                if (!httpContext.Response.HasStarted)
                {
                    await _exceptionHandlerService.ExceptionHandle(httpContext, exception);
                }
            }
        }
    }
}
