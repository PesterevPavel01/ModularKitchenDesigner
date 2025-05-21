using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;
using Result;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ModuleTypeController : ControllerBase
    {
        public ModuleTypeController(ISimpleEntityProcessorFactory simpleEntityService, IProcessorFactory<ModuleType, SimpleDto> moduleTypeProcessorFactory)
        {
            _moduleTypeProcessorFactory = moduleTypeProcessorFactory;
            _simpleEntityService = simpleEntityService;
            _loader = simpleEntityService.GetLoaderProcessor <SimpleEntityLoaderProcessor<ModuleType>, ModuleType>();
            _removeProcessor = simpleEntityService.GetRemoveProcessor<ModuleType>();
        }

        private readonly IProcessorFactory<ModuleType, SimpleDto> _moduleTypeProcessorFactory;
        private readonly ISimpleEntityProcessorFactory _simpleEntityService;

        private readonly ILoaderProcessor<ModuleType, SimpleDto> _loader;
        private readonly ISimpleEntityRemoveProcessor _removeProcessor;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _loader.ProcessAsync());

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(await _loader.ProcessAsync(predicate: x => x.Code == code));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _loader.ProcessAsync(predicate: x => x.Title == name));

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _moduleTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                    findEntityByDto: model => entity => model.GetId() == entity.Id));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _moduleTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.Code).Contains(entity.Code),
                    findEntityByDto: model => entity => model.Code == entity.Code));
    }
}