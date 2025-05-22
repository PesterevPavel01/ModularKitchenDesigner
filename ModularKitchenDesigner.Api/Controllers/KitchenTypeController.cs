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
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>>().ProcessAsync(predicate: x=> x.PriceSegment.Code == priceSegmentCode));

        [HttpGet("GetByPriceSegment/{priceSegmentTitle}")]

        public async Task<IActionResult> GetByPriceSegmentTitle(String priceSegmentTitle)
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>>().ProcessAsync(predicate: x => x.PriceSegment.Title == priceSegmentTitle));

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>>().ProcessAsync());

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<KitchenTypeDto> models)
            => Ok(
                await _kitchenTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<KitchenType, KitchenTypeDto, KitchenTypeConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                    findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<KitchenTypeDto> models)
            => Ok(
                await _kitchenTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<KitchenType, KitchenTypeDto, KitchenTypeConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                    findEntityByDto: model => entity => entity.Code == model.Code
                    ));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<KitchenTypeDto> models)
        => Ok(
            await _kitchenTypeProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<KitchenType, KitchenTypeDto>>()
            .ProcessAsync(
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code)));
    }
}
