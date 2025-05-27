using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("5.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ModuleTypeController : ControllerBase
    {
        public ModuleTypeController(ISimpleEntityProcessorFactory simpleEntityService, IProcessorFactory moduleTypeProcessorFactory)
        {
            _moduleTypeProcessorFactory = moduleTypeProcessorFactory;
            _removeProcessor = simpleEntityService.GetRemoveProcessor<ModuleType>();
        }

        private readonly IProcessorFactory _moduleTypeProcessorFactory;
        private readonly ISimpleEntityRemoveProcessor _removeProcessor;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _moduleTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModuleType, SimpleDto>, ModuleType, SimpleDto>().ProcessAsync());

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(await _moduleTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModuleType, SimpleDto>, ModuleType, SimpleDto>().ProcessAsync(predicate: x => x.Code == code));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _moduleTypeProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModuleType, SimpleDto>, ModuleType, SimpleDto>().ProcessAsync(predicate: x => x.Title == name));

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _moduleTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>, ModuleType, SimpleDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _moduleTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>, ModuleType, SimpleDto>()
                .ProcessAsync(models));
    }
}