using Microsoft.AspNetCore.Http;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Handlers
{
    public interface IExceptionHandlerService
    {
        Task ExceptionHandle(HttpContext httpContext, Exception exception);
    }
}
