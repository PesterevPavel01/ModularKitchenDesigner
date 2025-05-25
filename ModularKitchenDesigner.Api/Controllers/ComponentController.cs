using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("3.0")]
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
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Component, ComponentDto>>().ProcessAsync());

        [HttpGet("GetByPriceSegmentCode/{priceSegmentCode}")]
        public async Task<IActionResult> GetByPriceSegmentCode(String priceSegmentCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Component, ComponentDto>>().ProcessAsync(predicate: x => x.PriceSegment.Code == priceSegmentCode));

        [HttpGet("GetByPriceSegment/{priceSegmentTitle}")]
        public async Task<IActionResult> GetByPriceSegmentTitle(String priceSegmentTitle)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Component, ComponentDto>>().ProcessAsync(predicate: x => x.PriceSegment.Title == priceSegmentTitle));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ComponentDto> models)
            => Ok(
                await _componentProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<Component, ComponentDto, ComponentConverter>>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ComponentDto> models)
            => Ok(
                await _componentProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Component, ComponentDto, ComponentConverter>>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ComponentDto> models)
        => Ok(
            await _componentProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<Component, ComponentDto>>()
            .ProcessAsync(models));
    }
}
