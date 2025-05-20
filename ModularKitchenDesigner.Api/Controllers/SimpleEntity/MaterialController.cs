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
    public class MaterialController : ControllerBase
    {
        public MaterialController(ISimpleEntityProcessorFactory simpleEntityService, IProcessorFactory<Material, SimpleDto> materialProcessorFactory)
        {
            _materialProcessorFactory = materialProcessorFactory;
            _simpleEntityService = simpleEntityService;
            _creator = simpleEntityService.GetCreatorProcessor<SimpleEntityCreatorProcessor<Material>, BaseResult<SimpleDto>, SimpleDto>();
            _updater = simpleEntityService.GetUpdaterProcessor<Material>();
            _loader = simpleEntityService.GetLoaderProcessor<SimpleEntityLoaderProcessor<Material>, Material>();
            _removeProcessor = simpleEntityService.GetRemoveProcessor<Material>();
        }

        private readonly ISimpleEntityProcessorFactory _simpleEntityService;
        private readonly IProcessorFactory<Material, SimpleDto> _materialProcessorFactory;

        private readonly ICreatorProcessor<SimpleDto, BaseResult<SimpleDto>> _creator;
        private readonly IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, Material> _updater;
        private readonly ILoaderProcessor<Material, SimpleDto> _loader;
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
            => Ok(await _updater.ProcessAsync(model,null));

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _simpleEntityService.GetCreatorProcessor<SimleEntityMultipleCreatorProcessor<Material>, CollectionResult<SimpleDto>, List<SimpleDto>>().ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _materialProcessorFactory.GetUpdaterProcessor<CommonMultipleUpdaterProcessor<Material, SimpleDto, SimpleEntityConverter<Material>>, CollectionResult<SimpleDto>>()
                .ProcessAsync(
                data: models,
                predicate: entity => models.Select(model => model.Code).Contains(entity.Code)));

    }
}
