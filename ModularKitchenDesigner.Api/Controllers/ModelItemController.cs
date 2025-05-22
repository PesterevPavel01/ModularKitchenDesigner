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

    public class ModelItemController : ControllerBase
    {
        public ModelItemController(IProcessorFactory<ModelItem, ModelItemDto> modelItemProcessorFactory)
        {
            _modelItemProcessorFactory = modelItemProcessorFactory;
        }

        private readonly IProcessorFactory<ModelItem, ModelItemDto> _modelItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync());

        [HttpGet("GetByModule/{ModuleTitle}")]
        public async Task<IActionResult> GetByModule(String ModuleTitle)
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync(predicate: x => x.Module.Title == ModuleTitle));

        [HttpGet("GetByModuleCode/{ModuleCode}")]
        public async Task<IActionResult> GetByModuleModuleCode(String ModuleCode)
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync(predicate: x => x.Module.Code == ModuleCode));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<ModelItem, ModelItemDto, ModelItemConverter>>()
        .ProcessAsync(
            data: models,
            predicate: entity => 
                models.Select(model => model.ModuleCode).Contains(entity.Module.Code)
                && models.Select(model => model.ModelCode).Contains(entity.Model.Code),
            findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<ModelItem, ModelItemDto, ModelItemConverter>>()
            .ProcessAsync(
                data: models,
                predicate: entity =>
                    models.Select(model => model.ModuleCode).Contains(entity.Module.Code)
                    && models.Select(model => model.ModelCode).Contains(entity.Model.Code),
                findEntityByDto: model => entity => 
                    model.ModuleCode == entity.Module.Code
                    && model.ModelCode == entity.Model.Code));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<ModelItem, ModelItemDto>>()
            .ProcessAsync(
                predicate: entity =>
                    models.Select(model => model.ModuleCode).Contains(entity.Module.Code)
                    && models.Select(model => model.ModelCode).Contains(entity.Model.Code)));

    }
}