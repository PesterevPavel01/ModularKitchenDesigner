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
    public class ComponentTypeController : ControllerBase
    {
        public ComponentTypeController(ISimpleEntityProcessorFactory simpleEntityProcessorFactory, IProcessorFactory<ComponentType, SimpleDto> componentTypeProcessorFactory)
        {
            _componentTypeProcessorFactory = componentTypeProcessorFactory;
            _simpleEntityProcessorFactory = simpleEntityProcessorFactory;
            _creator = simpleEntityProcessorFactory.GetCreatorProcessor<SimpleEntityCreatorProcessor<ComponentType>, BaseResult<SimpleDto>, SimpleDto>();
            _updater = simpleEntityProcessorFactory.GetUpdaterProcessor<ComponentType>();
            _loader = simpleEntityProcessorFactory.GetLoaderProcessor <SimpleEntityLoaderProcessor<ComponentType>, ComponentType>();
            _removeProcessor = simpleEntityProcessorFactory.GetRemoveProcessor<ComponentType>();
        }

        private readonly ISimpleEntityProcessorFactory _simpleEntityProcessorFactory;
        private readonly IProcessorFactory<ComponentType, SimpleDto> _componentTypeProcessorFactory;

        private readonly ICreatorProcessor<SimpleDto, BaseResult<SimpleDto>> _creator;
        private readonly IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, ComponentType> _updater;
        private readonly ILoaderProcessor<ComponentType, SimpleDto> _loader;
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SimpleDto model)
            => Ok(await _creator.ProcessAsync(model));

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SimpleDto model)
            => Ok(await _updater.ProcessAsync(model, null));
        
        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _simpleEntityProcessorFactory.GetCreatorProcessor<SimleEntityMultipleCreatorProcessor<ComponentType>, CollectionResult<SimpleDto>, List<SimpleDto>>().ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _componentTypeProcessorFactory.GetUpdaterProcessor<CommonMultipleUpdaterProcessor<ComponentType, SimpleDto, SimpleEntityConverter<ComponentType>>, CollectionResult<SimpleDto>>()
                .ProcessAsync(
                data:models,
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code)));
    }
}
