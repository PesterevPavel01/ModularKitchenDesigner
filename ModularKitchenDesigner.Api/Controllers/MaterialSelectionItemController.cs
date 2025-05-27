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

    public class MaterialSelectionItemController : ControllerBase
    {
        public MaterialSelectionItemController(IProcessorFactory materialSelectionItemProcessorFactory)
        {
            _materialSelectionItemProcessorFactory = materialSelectionItemProcessorFactory;
        }

        private readonly IProcessorFactory _materialSelectionItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>().ProcessAsync());

        [HttpGet("GetByKitchenType/{KitchenTypeTitle}")]
        public async Task<IActionResult> GetByKitchenType(String KitchenTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>().ProcessAsync(predicate: x => x.KitchenType.Title == KitchenTypeTitle));

        [HttpGet("GetByComponentType/{ComponentTypeTitle}")]
        public async Task<IActionResult> GetByComponentType(String ComponentTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>().ProcessAsync(predicate: x => x.ComponentType.Title == ComponentTypeTitle));

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByComponentTypeCode/{ComponentTypeCode}")]
        public async Task<IActionResult> GetByComponentTypeCode(String ComponentTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>().ProcessAsync(predicate: x => x.ComponentType.Code == ComponentTypeCode));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSelectionItemDto> models)
            => Ok(
                await _materialSelectionItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>, MaterialSelectionItem, MaterialSelectionItemDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<MaterialSelectionItemDto> models)
            => Ok(
                await _materialSelectionItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>, MaterialSelectionItem, MaterialSelectionItemDto>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<MaterialSelectionItemDto> models)
        => Ok(
            await _materialSelectionItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>()
            .ProcessAsync(models));
    }
}
