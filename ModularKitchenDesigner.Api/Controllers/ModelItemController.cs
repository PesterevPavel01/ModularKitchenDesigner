using System;
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

    public class ModelItemController : ControllerBase
    {
        public ModelItemController(IProcessorFactory modelItemProcessorFactory)
        {
            _modelItemProcessorFactory = modelItemProcessorFactory;
        }

        private readonly IProcessorFactory _modelItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>, ModelItem, ModelItemDto>().ProcessAsync(predicate: x => x.Enabled == true));

        [HttpGet("GetByModule/{ModuleTitle}")]
        public async Task<IActionResult> GetByModule(String ModuleTitle)
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>, ModelItem, ModelItemDto>().ProcessAsync(predicate: x => x.Module.Title == ModuleTitle && x.Enabled == true));

        [HttpGet("GetByModuleCode/{ModuleCode}")]
        public async Task<IActionResult> GetByModuleModuleCode(String ModuleCode)
            => Ok(await _modelItemProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>, ModelItem, ModelItemDto>()
                .ProcessAsync(predicate: x => x.Module.Code == ModuleCode && x.Enabled == true));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<ModelItem, ModelItemDto, ModelItemConverter>, ModelItem, ModelItemDto>()
            .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<ModelItem, ModelItemDto, ModelItemConverter>, ModelItem, ModelItemDto>()
            .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ModelItemDto> models)
        => Ok(
            await _modelItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<ModelItem, ModelItemDto>, ModelItem, ModelItemDto>()
            .ProcessAsync(models));

    }
}