using ModularKitchenDesigner.Domain.Interfaces.Handlers;

namespace ModularKitchenDesigner.Api.Middlewares
{
    public class ExcertionHandlingMiddleware
    {
        private readonly RequestDelegate _next = null!;
        private readonly IExceptionHandlerService _exceptionHandlerService = null!;

        public ExcertionHandlingMiddleware(RequestDelegate next, IExceptionHandlerService exceptionHandlerService)
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
                await _exceptionHandlerService.ExceptionHandle(httpContext, exception);
            }
        }
    }
}
