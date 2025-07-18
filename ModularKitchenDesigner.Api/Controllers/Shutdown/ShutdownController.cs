using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services;

namespace ModularKitchenDesigner.Api.Controllers.Shutdown
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ShutdownController : ControllerBase
    {
        private readonly CompleteShutdownService _shutdownService;

        public ShutdownController(CompleteShutdownService shutdownService)
        {
            _shutdownService = shutdownService;
        }

        [HttpPost("All")]
        public async Task<IActionResult> ShutdownAll([FromBody] String body)
        {
            if (body == "ShutdownAll")
            {
                await _shutdownService.ShutdownAll();
                return Ok("All entities deactivated");
            }
            else
                return BadRequest();
        }

    }
}
