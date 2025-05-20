using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.ModuleProcessors.ModuleCreators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ModuleDto model)
            => Ok(await _moduleProcessorFactory.GetCreatorProcessor<SingleModuleCreatorProcessor, BaseResult<ModuleDto>, ModuleDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModuleDto> models)
            => Ok(await _moduleProcessorFactory.GetCreatorProcessor<MultipleModuleCreatorProcessor, CollectionResult<ModuleDto>, List<ModuleDto>>().ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ModuleDto> models)
            => Ok(
                await _moduleProcessorFactory
                .GetUpdaterProcessor<CommonMultipleUpdaterProcessor<Module, ModuleDto, ModuleConverter>, CollectionResult<ModuleDto>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.Code).Contains(entity.Code)));
    }
}