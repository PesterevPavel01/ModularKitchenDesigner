using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class OrderController : ControllerBase
    {
        /*
        public OrderController(IProcessorFactory<Section, OrderDto> orderProcessorFactory)
        {
            _orderProcessorFactory = orderProcessorFactory;
        }

        private readonly IProcessorFactory<Section, OrderDto> _orderProcessorFactory;
        */
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

    }
}