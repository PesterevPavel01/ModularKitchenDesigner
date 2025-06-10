using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CustomKitchenProcessor;

namespace ModularKitchenDesigner.Api.Controllers.KustomKitchen
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class KustomKitchenController : ControllerBase
    {
        private readonly CustomKitchenInformationProcessor _customKitchenInformationProcessor;
        public KustomKitchenController(CustomKitchenInformationProcessor customKitchenInformationProcessor)
        {
            _customKitchenInformationProcessor = customKitchenInformationProcessor;
        }

        [HttpGet("GetByCode/{KitchenCode}")]
        public async Task<IActionResult> GetAll(string KitchenCode)
            => Ok(await _customKitchenInformationProcessor.ProcessAsync(new() {KitchenCode = KitchenCode }));

    }
}
