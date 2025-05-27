using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("5.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PriceSegmentController : ControllerBase
    {
        public PriceSegmentController(ISimpleEntityProcessorFactory simpleEntityService, IProcessorFactory priceSegmentProcessorFactory)
        {
            _priceSegmentProcessorFactory = priceSegmentProcessorFactory;
            _removeProcessor = simpleEntityService.GetRemoveProcessor<PriceSegment>();
        }

        private readonly IProcessorFactory _priceSegmentProcessorFactory;
        private readonly ISimpleEntityRemoveProcessor _removeProcessor;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _priceSegmentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<PriceSegment, SimpleDto>, PriceSegment, SimpleDto> ().ProcessAsync(predicate: x => x.Title != "default"));

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(await _priceSegmentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<PriceSegment, SimpleDto>, PriceSegment, SimpleDto>().ProcessAsync(predicate: x => x.Code == code));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _priceSegmentProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<PriceSegment, SimpleDto>, PriceSegment, SimpleDto>().ProcessAsync(predicate: x => x.Title == name));

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _priceSegmentProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<PriceSegment, SimpleDto, SimpleEntityConverter<PriceSegment>>, PriceSegment, SimpleDto> ()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _priceSegmentProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<PriceSegment, SimpleDto, SimpleEntityConverter<PriceSegment>>, PriceSegment, SimpleDto>()
                .ProcessAsync(models));
    }
}
