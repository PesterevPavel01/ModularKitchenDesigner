using Microsoft.AspNetCore.Http;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Logging
{
    public interface ILogService
    {
        Task<BaseResult> LogAsync(HttpContext httpContext);
    }
}
