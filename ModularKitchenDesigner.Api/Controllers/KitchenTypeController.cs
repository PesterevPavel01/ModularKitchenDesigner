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
    [ApiVersion("4.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class KitchenTypeController : ControllerBase
    {
        public KitchenTypeController(IProcessorFactory kitchenTypeProcessorFactory )
        {
            _kitchenTypeProcessorFactory = kitchenTypeProcessorFactory;
        }

        private readonly IProcessorFactory _kitchenTypeProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet("GetByPriceSegmentCode/{priceSegmentCode}")]
        public async Task<IActionResult> GetByPriceSegmentCode(String priceSegmentCode)
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>, KitchenType, KitchenTypeDto>().ProcessAsync(predicate: x=> x.PriceSegment.Code == priceSegmentCode && x.Enabled == true));

        [HttpGet("GetByPriceSegment/{priceSegmentTitle}")]

        public async Task<IActionResult> GetByPriceSegmentTitle(String priceSegmentTitle)
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>, KitchenType, KitchenTypeDto>().ProcessAsync(predicate: x => x.PriceSegment.Title == priceSegmentTitle && x.Enabled == true));

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _kitchenTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>, KitchenType, KitchenTypeDto>().ProcessAsync(predicate: x => x.Enabled == true));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<KitchenTypeDto> models)
            => Ok(
                await _kitchenTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<KitchenType, KitchenTypeDto, KitchenTypeConverter>, KitchenType, KitchenTypeDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<KitchenTypeDto> models)
            => Ok(
                await _kitchenTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<KitchenType, KitchenTypeDto, KitchenTypeConverter>, KitchenType, KitchenTypeDto>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<KitchenTypeDto> models)
        => Ok(
            await _kitchenTypeProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<KitchenType, KitchenTypeDto>, KitchenType, KitchenTypeDto>()
            .ProcessAsync(models));
    }
}
