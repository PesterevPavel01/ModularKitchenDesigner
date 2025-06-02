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
    public class ComponentTypeController : ControllerBase
    {
        public ComponentTypeController(ISimpleEntityProcessorFactory simpleEntityProcessorFactory, IProcessorFactory componentTypeProcessorFactory)
        {
            _componentTypeProcessorFactory = componentTypeProcessorFactory;
            _removeProcessor = simpleEntityProcessorFactory.GetRemoveProcessor<ComponentType>();
        }

        private readonly IProcessorFactory _componentTypeProcessorFactory;

        private readonly ISimpleEntityRemoveProcessor _removeProcessor;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(
                await _componentTypeProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<ComponentType, SimpleDto>, ComponentType, SimpleDto>()
                .ProcessAsync(predicate: x => x.Title != "default" && x.Enabled == true));

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(
                await _componentTypeProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<ComponentType, SimpleDto>, ComponentType, SimpleDto>()
                .ProcessAsync(predicate: x => x.Code == code && x.Enabled == true));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(
                await _componentTypeProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<ComponentType, SimpleDto>, ComponentType, SimpleDto>()
                .ProcessAsync(predicate: x => x.Title == name && x.Enabled == true));
        
        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _componentTypeProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<ComponentType, SimpleDto, SimpleEntityConverter<ComponentType>>, ComponentType, SimpleDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _componentTypeProcessorFactory.GetCreatorProcessor<CommonMultipleUpdaterProcessor<ComponentType, SimpleDto, SimpleEntityConverter<ComponentType>>, ComponentType, SimpleDto>()
                .ProcessAsync(models));
    }
}
