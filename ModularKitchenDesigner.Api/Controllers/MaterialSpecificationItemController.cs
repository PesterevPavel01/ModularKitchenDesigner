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

    public class MaterialSpecificationItemController : ControllerBase
    {
        public MaterialSpecificationItemController(IProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto> materialSpecificationItemProcessorFactory)
        {
            _materialSpecificationItemProcessorFactory = materialSpecificationItemProcessorFactory;
        }

        private readonly IProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto> _materialSpecificationItemProcessorFactory;

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSpecificationItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>>().ProcessAsync());
        
        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity =>
                        models.Select(model => model.ModuleType).Contains(entity.ModuleType.Title)
                        && models.Select(model => model.MaterialSelectionItemGuid).Contains(entity.MaterialSelectionItem.Id)
                        && models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id),
                    findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity =>
                        models.Select(model => model.ModuleType).Contains(entity.ModuleType.Title)
                        && models.Select(model => model.MaterialSelectionItemGuid).Contains(entity.MaterialSelectionItem.Id)
                        && models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id),
                    findEntityByDto: model => entity =>
                        model.ModuleType == entity.ModuleType.Title
                        && model.MaterialSelectionItemGuid == entity.MaterialSelectionItem.Id
                        && model.KitchenGuid == entity.Kitchen.Id));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<MaterialSpecificationItemDto> models)
        => Ok(
            await _materialSpecificationItemProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>>()
            .ProcessAsync(
                predicate: entity =>
                    models.Select(model => model.ModuleType).Contains(entity.ModuleType.Title)
                    && models.Select(model => model.MaterialSelectionItemGuid).Contains(entity.MaterialSelectionItem.Id)
                    && models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id)));
    }
}
