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
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class KitchenController : ControllerBase
    {
        public KitchenController(IProcessorFactory kitchenProcessorFactory)
        {
            _kitchenProcessorFactory = kitchenProcessorFactory;
        }

        private readonly IProcessorFactory _kitchenProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen,KitchenDto>, Kitchen, KitchenDto>().ProcessAsync());

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>, Kitchen, KitchenDto>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByCode/{Code}")]
        public async Task<IActionResult> GetByCode(String Code)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>, Kitchen, KitchenDto>().ProcessAsync(predicate: x => x.UserId == Code));

        [HttpGet("GetByTitle/{Title}")]
        public async Task<IActionResult> GetByTitle(String Title)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>, Kitchen, KitchenDto>().ProcessAsync(predicate: x => x.UserLogin == Title));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<KitchenDto> models)
            => Ok(
                await _kitchenProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<Kitchen, KitchenDto, KitchenConverter>, Kitchen, KitchenDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<KitchenDto> models)
            => Ok(
                await _kitchenProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Kitchen, KitchenDto, KitchenConverter>, Kitchen, KitchenDto>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<KitchenDto> models)
        => Ok(
            await _kitchenProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<Kitchen, KitchenDto>, Kitchen, KitchenDto>()
            .ProcessAsync(models));
    }
}