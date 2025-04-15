using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenTypeLoader;
using ModularKitchenDesigner.Application.Services.Processors.KitchenTypeProcessors.KitchenTypeCreator;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class KitchenTypeController : ControllerBase
    {
        public KitchenTypeController(IProcessorFactory<KitchenType, KitchenTypeDto> kitchenTypeProcessorFactory )
        {
            _kitchenTypeProcessorFactory = kitchenTypeProcessorFactory;
        }

        private readonly IProcessorFactory<KitchenType, KitchenTypeDto> _kitchenTypeProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet("GetByPriceSegmentCode/{priceSegmentCode}")]
        public async Task<IActionResult> GetByPriceSegmentCode(String priceSegmentCode)
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<DefaultKitchenTypeLoaderProcessor>().ProcessAsync(predicate: x=> x.PriceSegment.Code == priceSegmentCode));

        [HttpGet("GetByPriceSegment/{priceSegmentTitle}")]

        public async Task<IActionResult> GetByPriceSegmentTitle(String priceSegmentTitle)
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<DefaultKitchenTypeLoaderProcessor>().ProcessAsync(predicate: x => x.PriceSegment.Title == priceSegmentTitle));

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<DefaultKitchenTypeLoaderProcessor>().ProcessAsync());

        [HttpPost("Create")]

        public async Task<IActionResult> Create([FromBody] KitchenTypeDto model)
            => Ok(await _kitchenTypeProcessorFactory.GetCreatorProcessor<SingleKitchenTypeCreatorProcessor>().ProcessAsync(model));

    }
}
