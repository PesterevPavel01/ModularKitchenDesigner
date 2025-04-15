using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.ComponentProcessors.ComponentLoaders;
using ModularKitchenDesigner.Application.Services.Processors.ConponentProcessors.ConponentCreators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ComponentController : ControllerBase
    {
        public ComponentController(IProcessorFactory<Component, ComponentDto> componentProcessorFactory)
        {
            _componentProcessorFactory = componentProcessorFactory;
        }

        private readonly IProcessorFactory<Component, ComponentDto> _componentProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultComponentLoaderProcessor>().ProcessAsync());

        [HttpGet("GetByPriceSegmentCode/{priceSegmentCode}")]
        public async Task<IActionResult> GetByPriceSegmentCode(String priceSegmentCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultComponentLoaderProcessor>().ProcessAsync(predicate: x => x.PriceSegment.Code == priceSegmentCode));

        [HttpGet("GetByPriceSegment/{priceSegmentTitle}")]
        public async Task<IActionResult> GetByPriceSegmentTitle(String priceSegmentTitle)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultComponentLoaderProcessor>().ProcessAsync(predicate: x => x.PriceSegment.Title == priceSegmentTitle));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ComponentDto model)
            => Ok(await _componentProcessorFactory.GetCreatorProcessor<SingleComponentCreatorProcessor>().ProcessAsync(model));
    }
}
