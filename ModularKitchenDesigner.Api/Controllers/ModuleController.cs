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

    public class ModuleController : ControllerBase
    {
        public ModuleController(IProcessorFactory<Module, ModuleDto> moduleProcessorFactory)
        {
            _moduleProcessorFactory = moduleProcessorFactory;
        }

        private readonly IProcessorFactory<Module, ModuleDto> _moduleProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        => Ok(await _moduleProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Module, ModuleDto>>().ProcessAsync());

        [HttpGet("GetByType/{ModuleType}")]
        public async Task<IActionResult> GetByModuleType(String ModuleType)
        => Ok(await _moduleProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Module, ModuleDto>>().ProcessAsync(predicate: x => x.Type.Title == ModuleType));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<Module, ModuleDto, ModuleConverter>>()
            .ProcessAsync(
                data: models,
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Module, ModuleDto, ModuleConverter>>()
            .ProcessAsync(
                data: models,
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                findEntityByDto: model => entity => model.Code == entity.Code));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<Module, ModuleDto>>()
            .ProcessAsync(
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code)));
    }
}