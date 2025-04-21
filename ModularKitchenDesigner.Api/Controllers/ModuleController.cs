using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.ModuleProcessor.ModuleCreators;
using ModularKitchenDesigner.Application.Services.Processors.ModuleProcessor.ModuleLoaders;
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
        public ModuleController(IProcessorFactory<Module, ModuleDto> componentProcessorFactory)
        {
            _componentProcessorFactory = componentProcessorFactory;
        }

        private readonly IProcessorFactory<Module, ModuleDto> _componentProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultModuleLoaderProcessor>().ProcessAsync());

        [HttpGet("GetByPriceSegment/{ModuleType}")]
        public async Task<IActionResult> GetByModuleType(String ModuleType)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultModuleLoaderProcessor>().ProcessAsync(predicate: x => x.Type.Title == ModuleType));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ModuleDto model)
            => Ok(await _componentProcessorFactory.GetCreatorProcessor<SingleModuleCreatorProcessor>().ProcessAsync(model));
    }
}