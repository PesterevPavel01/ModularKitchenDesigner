using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CustomKitchenProcessor;

namespace ModularKitchenDesigner.Api.Controllers.CustomKitchen
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomKitchenController : ControllerBase
    {
        private readonly CustomKitchenInformationProcessor _customKitchenInformationProcessor;

        public CustomKitchenController(CustomKitchenInformationProcessor customKitchenInformationProcessor)
        {
            _customKitchenInformationProcessor = customKitchenInformationProcessor;
        }

        /// <summary>
        /// Метод для получения информации о кухне
        /// </summary>
        /// <param name="KitchenCode"></param>
        /// <returns></returns>

        [HttpGet("GetByCode/{KitchenCode}")]
        public async Task<IActionResult> GetAll(string KitchenCode)
            => Ok(await _customKitchenInformationProcessor.ProcessAsync(new() {KitchenCode = KitchenCode }));

    }
}
