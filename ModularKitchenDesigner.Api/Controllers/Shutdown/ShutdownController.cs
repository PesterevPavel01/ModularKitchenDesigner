using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services;

namespace ModularKitchenDesigner.Api.Controllers.Shutdown
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    [Authorize(Policy = "Administrator")]

    public class ShutdownController : ControllerBase
    {
        private readonly CompleteShutdownService _shutdownService;

        public ShutdownController(CompleteShutdownService shutdownService)
        {
            _shutdownService = shutdownService;
        }
        /// <summary>
        /// Сброс всех сущностей
        /// </summary>
        /// <returns></returns>
        [HttpPost("All")]
        public async Task<IActionResult> ShutdownAll()
            => Ok(await _shutdownService.ShutdownAll());
    }
}
