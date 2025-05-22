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

    public class MaterialSelectionItemController : ControllerBase
    {
        public MaterialSelectionItemController(IProcessorFactory<MaterialSelectionItem, MaterialSelectionItemDto> materialSelectionItemProcessorFactory)
        {
            _materialSelectionItemProcessorFactory = materialSelectionItemProcessorFactory;
        }

        private readonly IProcessorFactory<MaterialSelectionItem, MaterialSelectionItemDto> _materialSelectionItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync());

        [HttpGet("GetByKitchenType/{KitchenTypeTitle}")]
        public async Task<IActionResult> GetByKitchenType(String KitchenTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.KitchenType.Title == KitchenTypeTitle));

        [HttpGet("GetByComponentType/{ComponentTypeTitle}")]
        public async Task<IActionResult> GetByComponentType(String ComponentTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.ComponentType.Title == ComponentTypeTitle));

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByComponentTypeCode/{ComponentTypeCode}")]
        public async Task<IActionResult> GetByComponentTypeCode(String ComponentTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.ComponentType.Code == ComponentTypeCode));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSelectionItemDto> models)
            => Ok(
                await _materialSelectionItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => 
                        models.Select(model => model.KitchenType).Contains(entity.KitchenType.Title)
                        && models.Select(model => model.Material).Contains(entity.Material.Title)
                        && models.Select(model => model.ComponentType).Contains(entity.ComponentType.Title),
                    findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<MaterialSelectionItemDto> models)
            => Ok(
                await _materialSelectionItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity =>
                        models.Select(model => model.KitchenType).Contains(entity.KitchenType.Title)
                        && models.Select(model => model.Material).Contains(entity.Material.Title)
                        && models.Select(model => model.ComponentType).Contains(entity.ComponentType.Title),
                    findEntityByDto: model => entity => 
                        model.KitchenType == entity.KitchenType.Title
                        && model.Material == entity.Material.Title
                        && model.ComponentType == entity.ComponentType.Title));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<MaterialSelectionItemDto> models)
        => Ok(
            await _materialSelectionItemProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>()
            .ProcessAsync(
                    predicate: entity =>
                        models.Select(model => model.KitchenType).Contains(entity.KitchenType.Title)
                        && models.Select(model => model.Material).Contains(entity.Material.Title)
                        && models.Select(model => model.ComponentType).Contains(entity.ComponentType.Title)));
    }
}
