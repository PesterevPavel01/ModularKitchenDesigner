using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.OrdersProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class OrderController : ControllerBase
    {
        public OrderController(IProcessorFactory<Section, OrderDto> orderProcessorFactory)
        {
            _orderProcessorFactory = orderProcessorFactory;
        }

        private readonly IProcessorFactory<Section, OrderDto> _orderProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");


        [HttpPost("CustomKitchen")]
        public async Task<IActionResult> Create([FromBody] OrderDto model)
            => Ok(await _orderProcessorFactory.GetCreatorProcessor<OrderCreatorProcessor, BaseResult<OrderDto>, OrderDto>().ProcessAsync(model));

    }
}