using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("4.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MaterialController : ControllerBase
    {
        public MaterialController(ISimpleEntityProcessorFactory simpleEntityService, IProcessorFactory<Material, SimpleDto> materialProcessorFactory)
        {
            _materialProcessorFactory = materialProcessorFactory; 
            _removeProcessor = simpleEntityService.GetRemoveProcessor<Material>();
        }

        private readonly IProcessorFactory<Material, SimpleDto> _materialProcessorFactory;

        private readonly ISimpleEntityRemoveProcessor _removeProcessor;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Material, SimpleDto>>().ProcessAsync());

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(await _materialProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Material, SimpleDto>>().ProcessAsync(predicate: x => x.Code == code));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _materialProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Material, SimpleDto>>().ProcessAsync(predicate: x => x.Title == name));

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
            => Ok(await _removeProcessor.RemoveAsync(code));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SimpleDto> models)
            => Ok(
                await _materialProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<Material, SimpleDto, SimpleEntityConverter<Material>>>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<SimpleDto> models)
            => Ok(await _materialProcessorFactory.GetCreatorProcessor<CommonMultipleUpdaterProcessor<Material, SimpleDto, SimpleEntityConverter<Material>>>()
                .ProcessAsync(models));

    }
}
